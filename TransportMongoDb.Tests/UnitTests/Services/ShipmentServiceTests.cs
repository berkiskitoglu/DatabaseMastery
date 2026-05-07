using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.ShipmentDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.ShipmentServices;
using MongoDB.Driver;
using Moq;

namespace TransportMongoDb.Tests.UnitTests.Services
{
    public class ShipmentServiceTests
    {
        private readonly Mock<IGenericRepository<Shipment>> _mockRepository;
        private readonly ShipmentService _shipmentService;
        private readonly Mock<IMapper> _mockMapper;

        public ShipmentServiceTests()
        {
            _mockRepository = new Mock<IGenericRepository<Shipment>>();
            _mockMapper = new Mock<IMapper>();
            _shipmentService = new ShipmentService(_mockRepository.Object, _mockMapper.Object);
        }

        #region CreateShipmentAsync Tests

        [Fact]
        public async Task CreateShipmentAsync_Should_Call_Repository_CreateAsync_Once()
        {
            // Arrange
            var createDto = new CreateShipmentDto { TrackingNumber = "TRK-001", SenderName = "Ahmet Yılmaz" };

            _mockMapper.Setup(x => x.Map<Shipment>(createDto)).Returns(new Shipment
            {
                TrackingNumber = createDto.TrackingNumber,
                SenderName = createDto.SenderName
            });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Shipment>())).Returns(Task.CompletedTask);

            // Act
            await _shipmentService.CreateShipmentAsync(createDto);

            // Assert
            _mockRepository.Verify(x => x.CreateAsync(It.IsAny<Shipment>()), Times.Once);
        }

        [Fact]
        public async Task CreateShipmentAsync_Should_Pass_Correct_Data_To_Repository()
        {
            // Arrange
            var createDto = new CreateShipmentDto
            {
                TrackingNumber = "TRK-001",
                SenderName = "Ahmet Yılmaz",
                ReceiverName = "Mehmet Demir",
                CurrentStatus = "Gönderi Alındı"
            };

            _mockMapper.Setup(x => x.Map<Shipment>(createDto)).Returns(new Shipment
            {
                TrackingNumber = createDto.TrackingNumber,
                SenderName = createDto.SenderName,
                ReceiverName = createDto.ReceiverName,
                CurrentStatus = createDto.CurrentStatus
            });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Shipment>())).Returns(Task.CompletedTask);

            // Act
            await _shipmentService.CreateShipmentAsync(createDto);

            // Assert
            _mockRepository.Verify(x => x.CreateAsync(It.Is<Shipment>(s =>
                s.TrackingNumber == "TRK-001" &&
                s.SenderName == "Ahmet Yılmaz" &&
                s.ReceiverName == "Mehmet Demir" &&
                s.CurrentStatus == "Gönderi Alındı"
            )), Times.Once);
        }

        [Fact]
        public async Task CreateShipmentAsync_Should_Handle_Repository_Exception()
        {
            // Arrange
            var createDto = new CreateShipmentDto { TrackingNumber = "TRK-001" };

            _mockMapper.Setup(x => x.Map<Shipment>(It.IsAny<CreateShipmentDto>()))
                .Returns(new Shipment { TrackingNumber = "TRK-001" });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Shipment>()))
                .ThrowsAsync(new Exception("Database Connection Failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _shipmentService.CreateShipmentAsync(createDto));

            Assert.Equal("Database Connection Failed", exception.Message);
        }

        #endregion

        #region GetAllShipmentAsync Tests

        [Fact]
        public async Task GetAllShipmentAsync_Should_Call_Repository_GetAllAsync_Once()
        {
            // Arrange
            var shipments = new List<Shipment>
            {
                new Shipment { TrackingNumber = "TRK-001" },
                new Shipment { TrackingNumber = "TRK-002" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(shipments);
            _mockMapper.Setup(x => x.Map<List<ResultShipmentDto>>(shipments)).Returns(new List<ResultShipmentDto>());

            // Act
            await _shipmentService.GetAllShipmentAsync();

            // Assert
            _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllShipmentAsync_Should_Return_Mapped_Dtos()
        {
            // Arrange
            var shipments = new List<Shipment> { new Shipment { TrackingNumber = "TRK-001" } };
            var expectedDtos = new List<ResultShipmentDto> { new ResultShipmentDto { TrackingNumber = "TRK-001" } };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(shipments);
            _mockMapper.Setup(x => x.Map<List<ResultShipmentDto>>(shipments)).Returns(expectedDtos);

            // Act
            var result = await _shipmentService.GetAllShipmentAsync();

            // Assert
            Assert.Equal(expectedDtos, result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllShipmentAsync_Should_Return_Empty_List_When_No_Shipments()
        {
            // Arrange
            var emptyList = new List<Shipment>();

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(emptyList);
            _mockMapper.Setup(x => x.Map<List<ResultShipmentDto>>(emptyList)).Returns(new List<ResultShipmentDto>());

            // Act
            var result = await _shipmentService.GetAllShipmentAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetShipmentByIdAsync Tests

        [Fact]
        public async Task GetShipmentByIdAsync_Should_Call_Repository_GetByIdAsync_Once()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            var shipment = new Shipment { Id = id, TrackingNumber = "TRK-001" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(shipment);
            _mockMapper.Setup(x => x.Map<GetShipmentByIdDto>(shipment)).Returns(new GetShipmentByIdDto());

            // Act
            await _shipmentService.GetShipmentByIdAsync(id);

            // Assert
            _mockRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetShipmentByIdAsync_Should_Return_Mapped_Dto()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            var shipment = new Shipment { Id = id, TrackingNumber = "TRK-001", SenderName = "Ahmet Yılmaz" };
            var expectedDto = new GetShipmentByIdDto { TrackingNumber = "TRK-001", SenderName = "Ahmet Yılmaz" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(shipment);
            _mockMapper.Setup(x => x.Map<GetShipmentByIdDto>(shipment)).Returns(expectedDto);

            // Act
            var result = await _shipmentService.GetShipmentByIdAsync(id);

            // Assert
            Assert.Equal(expectedDto, result);
            Assert.Equal("TRK-001", result.TrackingNumber);
        }

        #endregion

        #region GetShipmentByTrackingNumberAsync Tests

        [Fact]
        public async Task GetShipmentByTrackingNumberAsync_Should_Call_Repository_GetByFilterAsync_Once()
        {
            // Arrange
            var trackingNumber = "TRK-001";
            var shipment = new Shipment { TrackingNumber = trackingNumber };

            _mockRepository.Setup(x => x.GetByFilterAsync(It.IsAny<FilterDefinition<Shipment>>())).ReturnsAsync(shipment);
            _mockMapper.Setup(x => x.Map<GetShipmentByIdDto>(shipment)).Returns(new GetShipmentByIdDto());

            // Act
            await _shipmentService.GetShipmentByTrackingNumberAsync(trackingNumber);

            // Assert
            _mockRepository.Verify(x => x.GetByFilterAsync(It.IsAny<FilterDefinition<Shipment>>()), Times.Once);
        }

        [Fact]
        public async Task GetShipmentByTrackingNumberAsync_Should_Return_Mapped_Dto()
        {
            // Arrange
            var trackingNumber = "TRK-001";
            var shipment = new Shipment { TrackingNumber = trackingNumber, SenderName = "Ahmet Yılmaz" };
            var expectedDto = new GetShipmentByIdDto { TrackingNumber = trackingNumber, SenderName = "Ahmet Yılmaz" };

            _mockRepository.Setup(x => x.GetByFilterAsync(It.IsAny<FilterDefinition<Shipment>>())).ReturnsAsync(shipment);
            _mockMapper.Setup(x => x.Map<GetShipmentByIdDto>(shipment)).Returns(expectedDto);

            // Act
            var result = await _shipmentService.GetShipmentByTrackingNumberAsync(trackingNumber);

            // Assert
            Assert.Equal(expectedDto, result);
            Assert.Equal("TRK-001", result.TrackingNumber);
        }

        #endregion

        #region UpdateShipmentAsync Tests

        [Fact]
        public async Task UpdateShipmentAsync_Should_Call_Repository_UpdateAsync_Once()
        {
            // Arrange
            var updateDto = new UpdateShipmentDto
            {
                Id = "507f1f77bcf86cd799439011",
                TrackingNumber = "TRK-001",
                SenderName = "Yeni İsim",
                CurrentStatus = "Yolda"
            };

            _mockMapper.Setup(x => x.Map<Shipment>(updateDto)).Returns(new Shipment
            {
                Id = updateDto.Id,
                TrackingNumber = updateDto.TrackingNumber,
                SenderName = updateDto.SenderName,
                CurrentStatus = updateDto.CurrentStatus
            });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Shipment>())).Returns(Task.CompletedTask);

            // Act
            await _shipmentService.UpdateShipmentAsync(updateDto);

            // Assert
            _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Shipment>()), Times.Once);
        }

        [Fact]
        public async Task UpdateShipmentAsync_Should_Pass_Correct_Data_To_Repository()
        {
            // Arrange
            var updateDto = new UpdateShipmentDto
            {
                Id = "507f1f77bcf86cd799439011",
                TrackingNumber = "TRK-001",
                SenderName = "Yeni İsim",
                CurrentStatus = "Yolda"
            };

            _mockMapper.Setup(x => x.Map<Shipment>(updateDto)).Returns(new Shipment
            {
                Id = updateDto.Id,
                TrackingNumber = updateDto.TrackingNumber,
                SenderName = updateDto.SenderName,
                CurrentStatus = updateDto.CurrentStatus
            });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Shipment>())).Returns(Task.CompletedTask);

            // Act
            await _shipmentService.UpdateShipmentAsync(updateDto);

            // Assert
            _mockRepository.Verify(x => x.UpdateAsync(It.Is<Shipment>(s =>
                s.Id == "507f1f77bcf86cd799439011" &&
                s.TrackingNumber == "TRK-001" &&
                s.SenderName == "Yeni İsim" &&
                s.CurrentStatus == "Yolda"
            )), Times.Once);
        }

        #endregion

        #region DeleteShipmentAsync Tests

        [Fact]
        public async Task DeleteShipmentAsync_Should_Call_Repository_DeleteAsync_Once()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            _mockRepository.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            await _shipmentService.DeleteShipmentAsync(id);

            // Assert
            _mockRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteShipmentAsync_Should_Pass_Correct_Id_To_Repository()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            string capturedId = null;

            _mockRepository.Setup(x => x.DeleteAsync(It.IsAny<string>()))
                .Callback<string>(receivedId => capturedId = receivedId)
                .Returns(Task.CompletedTask);

            // Act
            await _shipmentService.DeleteShipmentAsync(id);

            // Assert
            Assert.Equal(id, capturedId);
        }

        #endregion

        #region Count Tests

        [Fact]
        public async Task GetTotalShipmentCountAsync_Should_Call_Repository_CountDocumentsAsync_Once()
        {
            // Arrange
            _mockRepository.Setup(x => x.CountDocumentsAsync(It.IsAny<FilterDefinition<Shipment>>())).ReturnsAsync(5);

            // Act
            var result = await _shipmentService.GetTotalShipmentCountAsync();

            // Assert
            Assert.Equal(5, result);
            _mockRepository.Verify(x => x.CountDocumentsAsync(It.IsAny<FilterDefinition<Shipment>>()), Times.Once);
        }

        [Fact]
        public async Task GetDeliveredShipmentCountAsync_Should_Call_Repository_CountDocumentsAsync_Once()
        {
            // Arrange
            _mockRepository.Setup(x => x.CountDocumentsAsync(It.IsAny<FilterDefinition<Shipment>>())).ReturnsAsync(3);

            // Act
            var result = await _shipmentService.GetDeliveredShipmentCountAsync();

            // Assert
            Assert.Equal(3, result);
            _mockRepository.Verify(x => x.CountDocumentsAsync(It.IsAny<FilterDefinition<Shipment>>()), Times.Once);
        }

        [Fact]
        public async Task GetInDistributionShipmentCountAsync_Should_Call_Repository_CountDocumentsAsync_Once()
        {
            // Arrange
            _mockRepository.Setup(x => x.CountDocumentsAsync(It.IsAny<FilterDefinition<Shipment>>())).ReturnsAsync(2);

            // Act
            var result = await _shipmentService.GetInDistributionShipmentCountAsync();

            // Assert
            Assert.Equal(2, result);
            _mockRepository.Verify(x => x.CountDocumentsAsync(It.IsAny<FilterDefinition<Shipment>>()), Times.Once);
        }

        [Fact]
        public async Task GetDistinctDestinationCityCountAsync_Should_Call_Repository_GetDistinctAsync_Once()
        {
            // Arrange
            var cities = new List<string> { "Ankara", "İstanbul", "İzmir" };
            _mockRepository.Setup(x => x.GetDistinctAsync<string>(It.IsAny<string>(), It.IsAny<FilterDefinition<Shipment>>()))
                .ReturnsAsync(cities);

            // Act
            var result = await _shipmentService.GetDistinctDestinationCityCountAsync();

            // Assert
            Assert.Equal(3, result);
            _mockRepository.Verify(x => x.GetDistinctAsync<string>(It.IsAny<string>(), It.IsAny<FilterDefinition<Shipment>>()), Times.Once);
        }

        #endregion
    }
}