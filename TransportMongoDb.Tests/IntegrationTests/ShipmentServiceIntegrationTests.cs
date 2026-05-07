using DotNetEnv;
using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.ShipmentDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.ShipmentServices;
using MongoDB.Driver;
using Xunit;

namespace TransportMongoDb.Tests.IntegrationTests
{
    public class ShipmentServiceIntegrationTests : IAsyncLifetime
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _database;
        private IGenericRepository<Shipment> _repository;
        private IMapper _mapper;
        private IShipmentService _shipmentService;
        private string _connectionString;
        private readonly string _testDatabaseName;

        public ShipmentServiceIntegrationTests()
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

            var collection = _database.GetCollection<Shipment>("Shipments");
            _repository = new GenericRepository<Shipment>(collection);

            var expression = new MapperConfigurationExpression();
            expression.CreateMap<CreateShipmentDto, Shipment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Trackings, opt => opt.Ignore());
            expression.CreateMap<UpdateShipmentDto, Shipment>()
                .ForMember(dest => dest.Trackings, opt => opt.Ignore());
            expression.CreateMap<Shipment, ResultShipmentDto>();
            expression.CreateMap<Shipment, GetShipmentByIdDto>();

            var config = new MapperConfiguration(expression);
            _mapper = config.CreateMapper();

            _shipmentService = new ShipmentService(_repository, _mapper);

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
        public async Task CreateShipmentAsync_Should_Save_To_Database()
        {
            // Arrange
            var dto = new CreateShipmentDto
            {
                TrackingNumber = "TRK-001",
                SenderName = "Ahmet Yılmaz",
                ReceiverName = "Mehmet Demir",
                OriginCity = "İstanbul",
                OriginDistrict = "Kadıköy",
                DestinationCity = "Ankara",
                DestinationDistrict = "Çankaya",
                Address = "Test Adres",
                CurrentStatus = "Gönderi Alındı",
                CreatedDate = DateTime.UtcNow
            };

            // Act
            await _shipmentService.CreateShipmentAsync(dto);

            // Assert
            var all = await _repository.GetAllAsync();
            Assert.Single(all);
            Assert.Equal("TRK-001", all[0].TrackingNumber);
            Assert.Equal("Ahmet Yılmaz", all[0].SenderName);
        }

        [Fact]
        public async Task CreateShipmentAsync_Multiple_Should_Be_Independent()
        {
            // Arrange
            var dto1 = new CreateShipmentDto { TrackingNumber = "TRK-001", SenderName = "Gönderici 1", CurrentStatus = "Gönderi Alındı" };
            var dto2 = new CreateShipmentDto { TrackingNumber = "TRK-002", SenderName = "Gönderici 2", CurrentStatus = "Yolda" };

            // Act
            await _shipmentService.CreateShipmentAsync(dto1);
            await _shipmentService.CreateShipmentAsync(dto2);

            // Assert
            var all = await _repository.GetAllAsync();
            Assert.Equal(2, all.Count);
        }

        #endregion

        #region READ Tests

        [Fact]
        public async Task GetAllShipmentAsync_Should_Return_Empty_When_No_Data()
        {
            // Act
            var result = await _shipmentService.GetAllShipmentAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllShipmentAsync_Should_Return_All()
        {
            // Arrange
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-001", CurrentStatus = "Gönderi Alındı" });
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-002", CurrentStatus = "Yolda" });

            // Act
            var result = await _shipmentService.GetAllShipmentAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetShipmentByIdAsync_Should_Return_Correct_Shipment()
        {
            // Arrange
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-001", SenderName = "Test Gönderici", CurrentStatus = "Gönderi Alındı" });
            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            // Act
            var result = await _shipmentService.GetShipmentByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TRK-001", result.TrackingNumber);
            Assert.Equal("Test Gönderici", result.SenderName);
        }

        [Fact]
        public async Task GetShipmentByTrackingNumberAsync_Should_Return_Correct_Shipment()
        {
            // Arrange
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-UNIQUE", SenderName = "Test Gönderici", CurrentStatus = "Gönderi Alındı" });

            // Act
            var result = await _shipmentService.GetShipmentByTrackingNumberAsync("TRK-UNIQUE");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TRK-UNIQUE", result.TrackingNumber);
        }

        [Fact]
        public async Task GetShipmentByTrackingNumberAsync_Should_Return_Null_When_Not_Found()
        {
            // Act
            var result = await _shipmentService.GetShipmentByTrackingNumberAsync("TRK-NOTEXIST");

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region UPDATE Tests

        [Fact]
        public async Task UpdateShipmentAsync_Should_Update_Fields()
        {
            // Arrange
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto
            {
                TrackingNumber = "TRK-001",
                SenderName = "Eski İsim",
                CurrentStatus = "Gönderi Alındı"
            });

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            var updateDto = new UpdateShipmentDto
            {
                Id = id,
                TrackingNumber = "TRK-001",
                SenderName = "Yeni İsim",
                CurrentStatus = "Yolda"
            };

            // Act
            await _shipmentService.UpdateShipmentAsync(updateDto);

            // Assert
            var updated = await _repository.GetByIdAsync(id);
            Assert.Equal("Yeni İsim", updated.SenderName);
            Assert.Equal("Yolda", updated.CurrentStatus);
        }

        #endregion

        #region DELETE Tests

        [Fact]
        public async Task DeleteShipmentAsync_Should_Remove()
        {
            // Arrange
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-DELETE", CurrentStatus = "Gönderi Alındı" });
            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            // Act
            await _shipmentService.DeleteShipmentAsync(id);

            // Assert
            var remaining = await _repository.GetAllAsync();
            Assert.Empty(remaining);
        }

        #endregion

        #region COUNT Tests

        [Fact]
        public async Task GetTotalShipmentCountAsync_Should_Return_Correct_Count()
        {
            // Arrange
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-001", CurrentStatus = "Gönderi Alındı" });
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-002", CurrentStatus = "Yolda" });
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-003", CurrentStatus = "Teslim Edildi" });

            // Act
            var count = await _shipmentService.GetTotalShipmentCountAsync();

            // Assert
            Assert.Equal(3, count);
        }

        [Fact]
        public async Task GetDeliveredShipmentCountAsync_Should_Return_Only_Delivered()
        {
            // Arrange
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-001", CurrentStatus = "Teslim Edildi" });
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-002", CurrentStatus = "Teslim Edildi" });
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-003", CurrentStatus = "Yolda" });

            // Act
            var count = await _shipmentService.GetDeliveredShipmentCountAsync();

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task GetInDistributionShipmentCountAsync_Should_Return_Only_InDistribution()
        {
            // Arrange
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-001", CurrentStatus = "Dağıtımda" });
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-002", CurrentStatus = "Dağıtımda" });
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-003", CurrentStatus = "Teslim Edildi" });

            // Act
            var count = await _shipmentService.GetInDistributionShipmentCountAsync();

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task GetDistinctDestinationCityCountAsync_Should_Return_Unique_City_Count()
        {
            // Arrange
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-001", DestinationCity = "Ankara", CurrentStatus = "Yolda" });
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-002", DestinationCity = "Ankara", CurrentStatus = "Yolda" });
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-003", DestinationCity = "İstanbul", CurrentStatus = "Yolda" });
            await _shipmentService.CreateShipmentAsync(new CreateShipmentDto { TrackingNumber = "TRK-004", DestinationCity = "İzmir", CurrentStatus = "Yolda" });

            // Act
            var count = await _shipmentService.GetDistinctDestinationCityCountAsync();

            // Assert
            Assert.Equal(3, count);
        }

        #endregion
    }
}