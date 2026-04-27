using DotNetEnv;
using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.AboutDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.AboutServices;
using MongoDB.Driver;
using Xunit;

namespace TransportMongoDb.Tests.IntegrationTests
{
    public class AboutServiceIntegrationTests : IAsyncLifetime
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _database;
        private IGenericRepository<About> _repository;
        private IMapper _mapper;
        private IAboutService _aboutService;
        private string _connectionString;
        private readonly string _testDatabaseName;

        public AboutServiceIntegrationTests()
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

            var collection = _database.GetCollection<About>("Abouts");

            _repository = new GenericRepository<About>(collection);

            var expression = new MapperConfigurationExpression();

            expression.CreateMap<CreateAboutDto, About>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            expression.CreateMap<UpdateAboutDto, About>();
            expression.CreateMap<About, ResultAboutDto>();
            expression.CreateMap<About, GetAboutByIdDto>();

            var config = new MapperConfiguration(expression);
            _mapper = config.CreateMapper();

            _aboutService = new AboutService(_repository, _mapper);

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

        #region CREATE Tests

        [Fact]
        public async Task CreateAboutAsync_Should_Save_To_Database()
        {
            var createDto = new CreateAboutDto
            {
                Title = "Integration Test",
                Description = "Test Desc",
                ImageUrl = "test.jpg"
            };

            await _aboutService.CreateAboutAsync(createDto);

            var all = await _repository.GetAllAsync();
            Assert.Single(all);
            Assert.Equal("Integration Test", all[0].Title);
        }

        [Fact]
        public async Task CreateAboutAsync_Multiple_Should_Be_Independent()
        {
            var dto1 = new CreateAboutDto { Title = "About 1", ImageUrl = "1.jpg" };
            var dto2 = new CreateAboutDto { Title = "About 2", ImageUrl = "2.jpg" };

            await _aboutService.CreateAboutAsync(dto1);
            await _aboutService.CreateAboutAsync(dto2);

            var all = await _repository.GetAllAsync();
            Assert.Equal(2, all.Count);
        }

        #endregion

        #region READ Tests

        [Fact]
        public async Task GetAllAboutAsync_Should_Return_Empty_When_No_Data()
        {
            var result = await _aboutService.GetAllAboutAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAboutAsync_Should_Return_All()
        {
            await _aboutService.CreateAboutAsync(
                new CreateAboutDto { Title = "About 1", ImageUrl = "1.jpg" }
            );

            await _aboutService.CreateAboutAsync(
                new CreateAboutDto { Title = "About 2", ImageUrl = "2.jpg" }
            );

            var result = await _aboutService.GetAllAboutAsync();

            Assert.Equal(2, result.Count);
        }

        #endregion

        #region UPDATE Tests

        [Fact]
        public async Task UpdateAboutAsync_Should_Update()
        {
            await _aboutService.CreateAboutAsync(
                new CreateAboutDto { Title = "Original", ImageUrl = "orig.jpg" }
            );

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            var updateDto = new UpdateAboutDto
            {
                Id = id,
                Title = "Updated",
                Description = "Updated Desc",
                ImageUrl = "updated.jpg"
            };

            await _aboutService.UpdateAboutAsync(updateDto);

            var updated = await _repository.GetByIdAsync(id);
            Assert.Equal("Updated", updated.Title);
        }

        #endregion

        #region DELETE Tests

        [Fact]
        public async Task DeleteAboutAsync_Should_Remove()
        {
            await _aboutService.CreateAboutAsync(
                new CreateAboutDto { Title = "Delete This", ImageUrl = "delete.jpg" }
            );

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            await _aboutService.DeleteAboutAsync(id);

            var remaining = await _repository.GetAllAsync();
            Assert.Empty(remaining);
        }

        #endregion
    }
}