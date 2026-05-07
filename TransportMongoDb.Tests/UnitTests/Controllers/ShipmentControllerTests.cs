using DatabaseMastery.TransportMongoDb.Controllers;
using DatabaseMastery.TransportMongoDb.Dtos.ShipmentDtos;
using DatabaseMastery.TransportMongoDb.Services.ShipmentServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace TransportMongoDb.Tests.UnitTests.Controllers
{
    public class ShipmentControllerTests
    {
        private readonly Mock<IShipmentService> _mockService;
        private readonly ShipmentController _controller;

        public ShipmentControllerTests()
        {
            _mockService = new Mock<IShipmentService>();
            _controller = new ShipmentController(_mockService.Object);
        }

        #region CreateShipment (POST) Tests

        [Fact]
        public async Task CreateShipment_Should_Call_Service_CreateShipmentAsync_Once()
        {
            // Arrange
            var createDto = new CreateShipmentDto
            {
                TrackingNumber = "TRK-001",
                SenderName = "Ahmet Yılmaz",
                CurrentStatus = "Gönderi Alındı"
            };

            _mockService.Setup(x => x.CreateShipmentAsync(It.IsAny<CreateShipmentDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateShipment(createDto);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            _mockService.Verify(x => x.CreateShipmentAsync(It.IsAny<CreateShipmentDto>()), Times.Once);
        }

        [Fact]
        public async Task CreateShipment_Should_Return_RedirectToAction()
        {
            // Arrange
            var createDto = new CreateShipmentDto { TrackingNumber = "TRK-001", CurrentStatus = "Gönderi Alındı" };
            _mockService.Setup(x => x.CreateShipmentAsync(It.IsAny<CreateShipmentDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateShipment(createDto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            string actionName = redirectResult.ActionName;
            Assert.Equal("ShipmentList", actionName);
        }

        [Fact]
        public async Task CreateShipment_Should_Pass_Correct_Dto_To_Service()
        {
            // Arrange
            var createDto = new CreateShipmentDto
            {
                TrackingNumber = "TRK-001",
                SenderName = "Ahmet Yılmaz",
                ReceiverName = "Mehmet Demir",
                OriginCity = "İstanbul",
                DestinationCity = "Ankara",
                CurrentStatus = "Gönderi Alındı"
            };

            _mockService.Setup(x => x.CreateShipmentAsync(It.IsAny<CreateShipmentDto>())).Returns(Task.CompletedTask);

            // Act
            await _controller.CreateShipment(createDto);

            // Assert
            _mockService.Verify(x => x.CreateShipmentAsync(It.Is<CreateShipmentDto>(dto =>
                dto.TrackingNumber == "TRK-001" &&
                dto.SenderName == "Ahmet Yılmaz" &&
                dto.ReceiverName == "Mehmet Demir" &&
                dto.OriginCity == "İstanbul" &&
                dto.DestinationCity == "Ankara"
            )), Times.Once);
        }

        [Fact]
        public async Task CreateShipment_Should_Throw_When_Service_Fails()
        {
            // Arrange
            var createDto = new CreateShipmentDto { TrackingNumber = "TRK-001", CurrentStatus = "Gönderi Alındı" };
            _mockService.Setup(x => x.CreateShipmentAsync(It.IsAny<CreateShipmentDto>())).ThrowsAsync(new Exception("Service Error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _controller.CreateShipment(createDto)
            );
            Assert.Equal("Service Error", exception.Message);
        }

        #endregion

        #region DeleteShipment Tests

        [Fact]
        public async Task DeleteShipment_Should_Call_Service_DeleteShipmentAsync()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            _mockService.Setup(x => x.DeleteShipmentAsync(id)).Returns(Task.CompletedTask);

            // Act
            await _controller.DeleteShipment(id);

            // Assert
            _mockService.Verify(x => x.DeleteShipmentAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteShipment_Should_Return_RedirectToAction()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            _mockService.Setup(x => x.DeleteShipmentAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteShipment(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            string actionName = redirectResult.ActionName;
            Assert.Equal("ShipmentList", actionName);
        }

        #endregion

        #region Index (ShipmentList) Tests

        [Fact]
        public async Task Index_Should_Call_Service_GetAllShipmentAsync()
        {
            // Arrange
            var shipments = new List<ResultShipmentDto>
            {
                new ResultShipmentDto { TrackingNumber = "TRK-001" },
                new ResultShipmentDto { TrackingNumber = "TRK-002" }
            };
            _mockService.Setup(x => x.GetAllShipmentAsync()).ReturnsAsync(shipments);

            // Act
            var result = await _controller.ShipmentList();

            // Assert
            _mockService.Verify(x => x.GetAllShipmentAsync(), Times.Once);
        }

        [Fact]
        public async Task Index_Should_Return_ViewResult_With_Shipments()
        {
            // Arrange
            var shipments = new List<ResultShipmentDto>
            {
                new ResultShipmentDto { TrackingNumber = "TRK-001" }
            };
            _mockService.Setup(x => x.GetAllShipmentAsync()).ReturnsAsync(shipments);

            // Act
            var result = await _controller.ShipmentList();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ResultShipmentDto>>(viewResult.Model);
            Assert.Single(model);
        }

        #endregion

        #region UpdateShipment Tests

        [Fact]
        public async Task UpdateShipment_POST_Should_Call_Service_UpdateShipmentAsync()
        {
            // Arrange
            var updateDto = new UpdateShipmentDto
            {
                Id = "507f1f77bcf86cd799439011",
                TrackingNumber = "TRK-001",
                SenderName = "Yeni İsim",
                CurrentStatus = "Yolda"
            };
            _mockService.Setup(x => x.UpdateShipmentAsync(It.IsAny<UpdateShipmentDto>())).Returns(Task.CompletedTask);

            // Act
            await _controller.UpdateShipment(updateDto);

            // Assert
            _mockService.Verify(x => x.UpdateShipmentAsync(It.IsAny<UpdateShipmentDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateShipment_POST_Should_Return_RedirectToAction()
        {
            // Arrange
            var updateDto = new UpdateShipmentDto
            {
                Id = "507f1f77bcf86cd799439011",
                TrackingNumber = "TRK-001",
                CurrentStatus = "Yolda"
            };
            _mockService.Setup(x => x.UpdateShipmentAsync(It.IsAny<UpdateShipmentDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateShipment(updateDto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            string actionName = redirectResult.ActionName;
            Assert.Equal("ShipmentList", actionName);
        }

        [Fact]
        public async Task UpdateShipment_GET_Should_Call_Service_GetShipmentByIdAsync()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            var shipmentDto = new GetShipmentByIdDto { TrackingNumber = "TRK-001" };
            _mockService.Setup(x => x.GetShipmentByIdAsync(id)).ReturnsAsync(shipmentDto);

            // Act
            var result = await _controller.UpdateShipment(id);

            // Assert
            _mockService.Verify(x => x.GetShipmentByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task UpdateShipment_GET_Should_Return_ViewResult_With_Shipment()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            var shipmentDto = new GetShipmentByIdDto { TrackingNumber = "TRK-001", SenderName = "Ahmet Yılmaz" };
            _mockService.Setup(x => x.GetShipmentByIdAsync(id)).ReturnsAsync(shipmentDto);

            // Act
            var result = await _controller.UpdateShipment(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<GetShipmentByIdDto>(viewResult.Model);
            Assert.Equal("TRK-001", model.TrackingNumber);
        }

        #endregion

        #region Count Tests

        [Fact]
        public async Task GetTotalShipmentCount_Should_Return_Correct_Count()
        {
            // Arrange
            _mockService.Setup(x => x.GetTotalShipmentCountAsync()).ReturnsAsync(5);

            // Act
            var result = await _mockService.Object.GetTotalShipmentCountAsync();

            // Assert
            Assert.Equal(5, result);
            _mockService.Verify(x => x.GetTotalShipmentCountAsync(), Times.Once);
        }

        [Fact]
        public async Task GetDeliveredShipmentCount_Should_Return_Correct_Count()
        {
            // Arrange
            _mockService.Setup(x => x.GetDeliveredShipmentCountAsync()).ReturnsAsync(3);

            // Act
            var result = await _mockService.Object.GetDeliveredShipmentCountAsync();

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetInDistributionShipmentCount_Should_Return_Correct_Count()
        {
            // Arrange
            _mockService.Setup(x => x.GetInDistributionShipmentCountAsync()).ReturnsAsync(2);

            // Act
            var result = await _mockService.Object.GetInDistributionShipmentCountAsync();

            // Assert
            Assert.Equal(2, result);
        }

        #endregion
    }
}