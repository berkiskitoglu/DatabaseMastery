using DotNetEnv;
using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.BrandDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.BrandServices;
using MongoDB.Driver;
using Xunit;

namespace TransportMongoDb.Tests.IntegrationTests
{
    public class BrandServiceTests : IAsyncLifetime
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _database;
        private IGenericRepository<Brand> _repository;
        private IMapper _mapper;
        private IBrandService _brandService;
        private string _connectionString;
        private readonly string _testDatabaseName;

        public BrandServiceTests()
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

            var collection = _database.GetCollection<Brand>("Brands");
            _repository = new GenericRepository<Brand>(collection);

            var expression = new MapperConfigurationExpression();
            expression.CreateMap<CreateBrandDto, Brand>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            expression.CreateMap<UpdateBrandDto, Brand>();
            expression.CreateMap<Brand, ResultBrandDto>();
            expression.CreateMap<Brand, GetBrandByIdDto>();

            var config = new MapperConfiguration(expression);
            _mapper = config.CreateMapper();

            _brandService = new BrandService(_repository, _mapper);

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
        public async Task CreateBrandAsync_Should_Save_To_Database()
        {
            // Arrange
            var createDto = new CreateBrandDto
            {
                BrandName = "Integration Test Brand",
                ImageUrl = "test.jpg"
            };

            // Act
            await _brandService.CreateBrandAsync(createDto);

            // Assert
            var all = await _repository.GetAllAsync();
            Assert.Single(all);
            Assert.Equal("Integration Test Brand", all[0].BrandName);
        }

        [Fact]
        public async Task CreateBrandAsync_Multiple_Should_Be_Independent()
        {
            // Arrange
            var dto1 = new CreateBrandDto { BrandName = "Brand 1", ImageUrl = "1.jpg" };
            var dto2 = new CreateBrandDto { BrandName = "Brand 2", ImageUrl = "2.jpg" };

            // Act
            await _brandService.CreateBrandAsync(dto1);
            await _brandService.CreateBrandAsync(dto2);

            // Assert
            var all = await _repository.GetAllAsync();
            Assert.Equal(2, all.Count);
        }

        #endregion

        #region READ Tests

        [Fact]
        public async Task GetAllBrandAsync_Should_Return_Empty_When_No_Data()
        {
            // Act
            var result = await _brandService.GetAllBrandAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllBrandAsync_Should_Return_All()
        {
            // Arrange
            await _brandService.CreateBrandAsync(
                new CreateBrandDto { BrandName = "Brand 1", ImageUrl = "1.jpg" }
            );
            await _brandService.CreateBrandAsync(
                new CreateBrandDto { BrandName = "Brand 2", ImageUrl = "2.jpg" }
            );

            // Act
            var result = await _brandService.GetAllBrandAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        #endregion

        #region UPDATE Tests

        [Fact]
        public async Task UpdateBrandAsync_Should_Update()
        {
            // Arrange
            await _brandService.CreateBrandAsync(
                new CreateBrandDto { BrandName = "Original Brand", ImageUrl = "orig.jpg" }
            );

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            var updateDto = new UpdateBrandDto
            {
                Id = id,
                BrandName = "Updated Brand",
                ImageUrl = "updated.jpg"
            };

            // Act
            await _brandService.UpdateBrandAsync(updateDto);

            // Assert
            var updated = await _repository.GetByIdAsync(id);
            Assert.Equal("Updated Brand", updated.BrandName);
        }

        #endregion

        #region DELETE Tests

        [Fact]
        public async Task DeleteBrandAsync_Should_Remove()
        {
            // Arrange
            await _brandService.CreateBrandAsync(
                new CreateBrandDto { BrandName = "Delete This", ImageUrl = "delete.jpg" }
            );

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            // Act
            await _brandService.DeleteBrandAsync(id);

            // Assert
            var remaining = await _repository.GetAllAsync();
            Assert.Empty(remaining);
        }

        #endregion
    }
}