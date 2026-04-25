using AutoMapper;
using Castle.Core.Logging;
using DatabaseMastery.TransportMongoDb.Dtos.SliderDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Mapping;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.SliderServices;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Xunit;


namespace TransportMongoDb.Tests.IntegrationTests
{
    [Collection("Sequential")]
    public class SliderServiceTests : IAsyncLifetime
    {

        private IMongoClient _mongoClient;
        private IMongoDatabase _database;
        private IGenericRepository<Slider> _repository;
        private IMapper _mapper;
        private ISliderService _sliderService;
        private string _connectionString;
        private readonly string _testDatabaseName;

        public SliderServiceTests()
        {
            _testDatabaseName = "test_db_" + Guid.NewGuid().ToString();
            _connectionString =
    Environment.GetEnvironmentVariable("DatabaseSettings__ConnectionString")
    ?? "mongodb://localhost:27017";
        }

        // Her test öncesi çalışır
        public async Task InitializeAsync()
        {
            // MongoDB'ye bağlan
            _mongoClient = new MongoClient();
            _database = _mongoClient.GetDatabase(_testDatabaseName);

            // Repository oluştur 
            var collection = _database.GetCollection<Slider>("Sliders");

            _repository = new GenericRepository<Slider>(collection);

            // Mapper oluştur
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateSliderDto, Slider>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore());

                cfg.CreateMap<UpdateSliderDto, Slider>();

                cfg.CreateMap<Slider, ResultSliderDto>();
                cfg.CreateMap<Slider, GetSliderByIdDto>();
            }, loggerFactory);

            config.AssertConfigurationIsValid();

            _mapper = config.CreateMapper();

            // Service oluştur 
            _sliderService = new SliderService(_repository, _mapper);
        }

        // Her test sonrası çalışır (cleanup)
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
        public async Task CreateSliderAsync_Should_Save_To_Database()
        {
            // Arrange
            var createDto = new CreateSliderDto
            {
                Title = "Integration Test",
                SubTitle = "Test Sub",
                Description = "Test Desc",
                ImageUrl = "test.jpg"
            };

            // Act
            await _sliderService.CreateSliderAsync(createDto);

            //Assert
            var all = await _repository.GetAllAsync();
            Assert.Single(all);
            Assert.Equal("Integration Test", all[0].Title);

        }

        [Fact]
        public async Task CreateSliderAsync_Multiple_Should_Be_Independent()
        {
            // Arrange
            var dto1 = new CreateSliderDto { Title = "Slider 1", ImageUrl = "1.jpg" };
            var dto2 = new CreateSliderDto { Title = "Slider 2", ImageUrl = "2.jpg" };

            // Act
            await _sliderService.CreateSliderAsync(dto1);
            await _sliderService.CreateSliderAsync(dto2);

            // Assert
            var all = await _repository.GetAllAsync();
            Assert.Equal(2, all.Count);
        }
        #endregion

        #region READ Tests
        [Fact]
        public async Task GetAllSliderAsync_Should_Return_Empty_When_No_Data()
        {
            // Act
            var result = await _sliderService.GetAllSliderAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllSliderAsync_Should_Return_All()
        {
            // Arrange
            await _sliderService.CreateSliderAsync(
                new CreateSliderDto { Title = "Slider 1", ImageUrl = "1.jpg" }
            );
            await _sliderService.CreateSliderAsync(
                new CreateSliderDto { Title = "Slider 2", ImageUrl = "2.jpg" }
            );

            // Act
            var result = await _sliderService.GetAllSliderAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        #endregion

        #region UPDATE Tests
        [Fact]
        public async Task UpdateSliderAsync_Should_Update()
        {
            // Arrange
            await _sliderService.CreateSliderAsync(
                new CreateSliderDto { Title = "Original", ImageUrl = "orig.jpg" }
            );

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            var updateDto = new UpdateSliderDto
            {

                Id = id,
                Title = "Updated",
                ImageUrl = "updated.jpg"
            };

            // Act
            await _sliderService.UpdateSliderAsync(updateDto);

            // Assert
            var updated = await _repository.GetByIdAsync(id);
            Assert.Equal("Updated", updated.Title);
        }

        #endregion

        #region DELETE Tests
        [Fact]
        public async Task DeleteSliderAsync_Should_Remove()
        {
            // Arrange
            await _sliderService.CreateSliderAsync(new CreateSliderDto { Title = "Delete This", ImageUrl = "delete.jpg" });

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            // Act
            await _sliderService.DeleteSliderAsync(id);

            // Assert
            var remaining = await _repository.GetAllAsync();
            Assert.Empty(remaining);
        }
        #endregion

    }
}



