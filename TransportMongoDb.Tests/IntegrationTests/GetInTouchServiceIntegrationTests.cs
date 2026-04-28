using DotNetEnv;
using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.GetInTouchDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.GetInTouchServices;
using MongoDB.Driver;
using Xunit;

namespace TransportMongoDb.Tests.IntegrationTests
{
    public class GetInTouchServiceIntegrationTests : IAsyncLifetime
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _database;
        private IGenericRepository<GetInTouch> _repository;
        private IMapper _mapper;
        private IGetInTouchService _service;
        private string _connectionString;
        private readonly string _testDatabaseName;

        public GetInTouchServiceIntegrationTests()
        {
            var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var binPath = Path.GetDirectoryName(assemblyLocation);
            var testProjectPath = Directory.GetParent(binPath)?.Parent?.Parent?.FullName;
            var solutionFolder = Directory.GetParent(testProjectPath)?.FullName;

            var envPath = Path.Combine(solutionFolder ?? "", ".env");

            if (!File.Exists(envPath))
                throw new Exception($".env file not found at {envPath}");

            Env.Load(envPath);

            _testDatabaseName = "test_" + Guid.NewGuid().ToString("N")[..8];

            _connectionString =
                Environment.GetEnvironmentVariable("DatabaseSettings__ConnectionString")
                ?? throw new Exception("MongoDB connection string missing");
        }

        public Task InitializeAsync()
        {
            _mongoClient = new MongoClient(_connectionString);
            _database = _mongoClient.GetDatabase(_testDatabaseName);

            var collection = _database.GetCollection<GetInTouch>("GetInTouch");
            _repository = new GenericRepository<GetInTouch>(collection);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateGetInTouchDto, GetInTouch>()
                    .ForMember(x => x.Id, opt => opt.Ignore());

                cfg.CreateMap<UpdateGetInTouchDto, GetInTouch>();
                cfg.CreateMap<GetInTouch, ResultGetInTouchDto>();
                cfg.CreateMap<GetInTouch, GetInTouchByIdDto>();
            });

            _mapper = config.CreateMapper();
            _service = new GetInTouchService(_repository, _mapper);

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
            var dto = new CreateGetInTouchDto
            {
                MainTitle = "Test Title",
                Description = "Desc",
                ImageUrl = "img.jpg"
            };

            await _service.CreateGetInTouchAsync(dto);

            var all = await _repository.GetAllAsync();

            Assert.Single(all);
            Assert.Equal("Test Title", all[0].MainTitle);
        }

        #endregion

        #region READ

        [Fact]
        public async Task GetAll_Should_Return_Empty()
        {
            var result = await _service.GetAllGetInTouchAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_Should_Return_Data()
        {
            await _service.CreateGetInTouchAsync(new CreateGetInTouchDto { MainTitle = "A" });
            await _service.CreateGetInTouchAsync(new CreateGetInTouchDto { MainTitle = "B" });

            var result = await _service.GetAllGetInTouchAsync();

            Assert.Equal(2, result.Count);
        }

        #endregion

        #region UPDATE

        [Fact]
        public async Task Update_Should_Work()
        {
            await _service.CreateGetInTouchAsync(new CreateGetInTouchDto
            {
                MainTitle = "Old"
            });

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            var updateDto = new UpdateGetInTouchDto
            {
                Id = id,
                MainTitle = "New"
            };

            await _service.UpdateGetInTouchAsync(updateDto);

            var updated = await _repository.GetByIdAsync(id);

            Assert.Equal("New", updated.MainTitle);
        }

        #endregion

        #region DELETE

        [Fact]
        public async Task Delete_Should_Remove()
        {
            await _service.CreateGetInTouchAsync(new CreateGetInTouchDto
            {
                MainTitle = "Delete"
            });

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            await _service.DeleteGetInTouchAsync(id);

            var remaining = await _repository.GetAllAsync();

            Assert.Empty(remaining);
        }

        #endregion
    }
}