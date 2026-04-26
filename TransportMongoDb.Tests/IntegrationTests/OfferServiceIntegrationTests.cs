using DotNetEnv;
using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.OfferDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.OfferServices;
using MongoDB.Driver;
using Xunit;

namespace TransportMongoDb.Tests.IntegrationTests
{
    public class OfferServiceIntegrationTests : IAsyncLifetime
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _database;
        private IGenericRepository<Offer> _repository;
        private IMapper _mapper;
        private IOfferService _offerService;
        private string _connectionString;
        private readonly string _testDatabaseName;

        public OfferServiceIntegrationTests()
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

            var collection = _database.GetCollection<Offer>("Offers");
            _repository = new GenericRepository<Offer>(collection);

            var expression = new MapperConfigurationExpression();
            expression.CreateMap<CreateOfferDto, Offer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            expression.CreateMap<UpdateOfferDto, Offer>();
            expression.CreateMap<Offer, ResultOfferDto>();
            expression.CreateMap<Offer, GetOfferByIdDto>();

            var config = new MapperConfiguration(expression);
            _mapper = config.CreateMapper();

            _offerService = new OfferService(_repository, _mapper);

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
        public async Task CreateOfferAsync_Should_Save_To_Database()
        {
            // Arrange
            var createDto = new CreateOfferDto
            {
                Title = "Integration Test Offer",
                Description = "Test Desc",
                ImageUrl = "test.jpg"
            };

            // Act
            await _offerService.CreateOfferAsync(createDto);

            // Assert
            var all = await _repository.GetAllAsync();
            Assert.Single(all);
            Assert.Equal("Integration Test Offer", all[0].Title);
        }

        [Fact]
        public async Task CreateOfferAsync_Multiple_Should_Be_Independent()
        {
            // Arrange
            var dto1 = new CreateOfferDto { Title = "Offer 1", ImageUrl = "1.jpg" };
            var dto2 = new CreateOfferDto { Title = "Offer 2", ImageUrl = "2.jpg" };

            // Act
            await _offerService.CreateOfferAsync(dto1);
            await _offerService.CreateOfferAsync(dto2);

            // Assert
            var all = await _repository.GetAllAsync();
            Assert.Equal(2, all.Count);
        }

        #endregion

        #region READ Tests

        [Fact]
        public async Task GetAllOfferAsync_Should_Return_Empty_When_No_Data()
        {
            // Act
            var result = await _offerService.GetAllOfferAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllOfferAsync_Should_Return_All()
        {
            // Arrange
            await _offerService.CreateOfferAsync(
                new CreateOfferDto { Title = "Offer 1", ImageUrl = "1.jpg" }
            );
            await _offerService.CreateOfferAsync(
                new CreateOfferDto { Title = "Offer 2", ImageUrl = "2.jpg" }
            );

            // Act
            var result = await _offerService.GetAllOfferAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        #endregion

        #region UPDATE Tests

        [Fact]
        public async Task UpdateOfferAsync_Should_Update()
        {
            // Arrange
            await _offerService.CreateOfferAsync(
                new CreateOfferDto { Title = "Original Offer", ImageUrl = "orig.jpg" }
            );

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            var updateDto = new UpdateOfferDto
            {
                Id = id,
                Title = "Updated Offer",
                ImageUrl = "updated.jpg"
            };

            // Act
            await _offerService.UpdateOfferAsync(updateDto);

            // Assert
            var updated = await _repository.GetByIdAsync(id);
            Assert.Equal("Updated Offer", updated.Title);
        }

        #endregion

        #region DELETE Tests

        [Fact]
        public async Task DeleteOfferAsync_Should_Remove()
        {
            // Arrange
            await _offerService.CreateOfferAsync(
                new CreateOfferDto { Title = "Delete This", ImageUrl = "delete.jpg" }
            );

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            // Act
            await _offerService.DeleteOfferAsync(id);

            // Assert
            var remaining = await _repository.GetAllAsync();
            Assert.Empty(remaining);
        }

        #endregion
    }
}