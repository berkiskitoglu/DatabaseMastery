using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.HowItWorkDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.HowItWorkServices;
using DotNetEnv;
using MongoDB.Driver;
using Xunit;

namespace TransportMongoDb.Tests.IntegrationTests
{
    public class HowItWorkServiceIntegrationTests : IAsyncLifetime
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _database;
        private IGenericRepository<HowItWork> _repository;
        private IMapper _mapper;
        private IHowItWorkService _service;
        private string _connectionString;
        private readonly string _testDatabaseName;

        public HowItWorkServiceIntegrationTests()
        {
            var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var binPath = Path.GetDirectoryName(assemblyLocation);
            var testProjectPath = Directory.GetParent(binPath)?.Parent?.Parent?.FullName;
            var solutionFolder = Directory.GetParent(testProjectPath)?.FullName;

            var envPath = Path.Combine(solutionFolder ?? "", ".env");

            if (!File.Exists(envPath))
                throw new Exception($".env file not found at {envPath}");

            DotNetEnv.Env.Load(envPath);

            _testDatabaseName = "test_" + Guid.NewGuid().ToString("N")[..8];

            _connectionString =
                Environment.GetEnvironmentVariable("DatabaseSettings__ConnectionString")
                ?? throw new Exception("MongoDB connection string missing");
        }

        public Task InitializeAsync()
        {
            _mongoClient = new MongoClient(_connectionString);
            _database = _mongoClient.GetDatabase(_testDatabaseName);

            var collection = _database.GetCollection<HowItWork>("HowItWorks");
            _repository = new GenericRepository<HowItWork>(collection);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateHowItWorkDto, HowItWork>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore());

                cfg.CreateMap<UpdateHowItWorkDto, HowItWork>();
                cfg.CreateMap<HowItWork, ResultHowItWorkDto>();
                cfg.CreateMap<HowItWork, GetHowItWorkByIdDto>();
            });

            _mapper = config.CreateMapper();

            _service = new HowItWorkService(_repository, _mapper);

            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            if (_mongoClient != null)
            {
                await _mongoClient.DropDatabaseAsync(_testDatabaseName);
                _mongoClient.Dispose();
            }
        }

        #region CREATE

        [Fact]
        public async Task Create_Should_Save()
        {
            var dto = new CreateHowItWorkDto
            {
                Title = "Step 1",
                Description = "Desc",
                IconUrl = "icon.png",
                Status = true
            };

            await _service.CreateHowItWorkAsync(dto);

            var all = await _repository.GetAllAsync();

            Assert.Single(all);
            Assert.Equal("Step 1", all[0].Title);
        }

        #endregion

        #region READ

        [Fact]
        public async Task GetAll_Should_Return_Empty()
        {
            var result = await _service.GetAllHowItWorkAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_Should_Return_Data()
        {
            await _service.CreateHowItWorkAsync(new CreateHowItWorkDto { Title = "A" });
            await _service.CreateHowItWorkAsync(new CreateHowItWorkDto { Title = "B" });

            var result = await _service.GetAllHowItWorkAsync();

            Assert.Equal(2, result.Count);
        }

        #endregion

        #region UPDATE

        [Fact]
        public async Task Update_Should_Work()
        {
            await _service.CreateHowItWorkAsync(new CreateHowItWorkDto { Title = "Old" });

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            var dto = new UpdateHowItWorkDto
            {
                Id = id,
                Title = "New"
            };

            await _service.UpdateHowItWorkAsync(dto);

            var updated = await _repository.GetByIdAsync(id);

            Assert.Equal("New", updated.Title);
        }

        #endregion

        #region DELETE

        [Fact]
        public async Task Delete_Should_Remove()
        {
            await _service.CreateHowItWorkAsync(new CreateHowItWorkDto { Title = "Delete" });

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            await _service.DeleteHowItWorkAsync(id);

            var remaining = await _repository.GetAllAsync();

            Assert.Empty(remaining);
        }

        #endregion
    }
}