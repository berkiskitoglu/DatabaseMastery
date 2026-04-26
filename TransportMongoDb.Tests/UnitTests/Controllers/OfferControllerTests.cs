using DatabaseMastery.TransportMongoDb.Controllers;
using DatabaseMastery.TransportMongoDb.Dtos.OfferDtos;
using DatabaseMastery.TransportMongoDb.Services.OfferServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransportMongoDb.Tests.UnitTests.Controllers
{
    public class OfferControllerTests
    {
        private readonly Mock<IOfferService> _mockService;
        private readonly OfferController _controller;

        public OfferControllerTests()
        {
            _mockService = new Mock<IOfferService>();
            _controller = new OfferController(_mockService.Object);
        }

        #region CreateOffer (POST) Tests

        [Fact]
        public async Task CreateOffer_Should_Call_Service_CreateOfferAsync_Once()
        {
            // Arrange
            var createDto = new CreateOfferDto
            {
                Title = "Test Offer",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateOfferAsync(It.IsAny<CreateOfferDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateOffer(createDto);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            _mockService.Verify(x => x.CreateOfferAsync(It.IsAny<CreateOfferDto>()), Times.Once);
        }

        [Fact]
        public async Task CreateOffer_Should_Return_RedirectToAction()
        {
            // Arrange
            var createDto = new CreateOfferDto
            {
                Title = "Test Offer",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateOfferAsync(It.IsAny<CreateOfferDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateOffer(createDto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("OfferList", redirectResult.ActionName);
        }

        [Fact]
        public async Task CreateOffer_Should_Pass_Correct_Dto_To_Service()
        {
            // Arrange
            var createDto = new CreateOfferDto
            {
                Title = "Test Offer",
                Description = "Test Description",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateOfferAsync(It.IsAny<CreateOfferDto>())).Returns(Task.CompletedTask);

            // Act
            await _controller.CreateOffer(createDto);

            // Assert
            _mockService.Verify(x => x.CreateOfferAsync(It.Is<CreateOfferDto>(dto =>
                dto.Title == "Test Offer" &&
                dto.Description == "Test Description" &&
                dto.ImageUrl == "test.jpg"
            )), Times.Once());
        }

        [Fact]
        public async Task CreateOffer_Should_Throw_When_Service_Fails()
        {
            // Arrange
            var createDto = new CreateOfferDto
            {
                Title = "Test Offer",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateOfferAsync(It.IsAny<CreateOfferDto>())).ThrowsAsync(new Exception("Service Error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _controller.CreateOffer(createDto)
            );

            Assert.Equal("Service Error", exception.Message);
        }

        #endregion

        #region DeleteOffer Tests

        [Fact]
        public async Task DeleteOffer_Should_Call_Service_DeleteOfferAsync()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteOfferAsync(id)).Returns(Task.CompletedTask);

            // Act
            await _controller.DeleteOffer(id);

            // Assert
            _mockService.Verify(x => x.DeleteOfferAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteOffer_Should_Return_Redirect_To_Action()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteOfferAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteOffer(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("OfferList", redirectResult.ActionName);
        }

        #endregion

        #region OfferList Tests

        [Fact]
        public async Task OfferList_Should_Call_Service_GetAllOfferAsync()
        {
            // Arrange
            var offers = new List<ResultOfferDto>
            {
                new ResultOfferDto { Title = "Offer 1", ImageUrl = "img1.jpg" },
                new ResultOfferDto { Title = "Offer 2", ImageUrl = "img2.jpg" }
            };

            _mockService.Setup(x => x.GetAllOfferAsync()).ReturnsAsync(offers);

            // Act
            var result = await _controller.OfferList();

            // Assert
            _mockService.Verify(x => x.GetAllOfferAsync(), Times.Once);
        }

        [Fact]
        public async Task OfferList_Should_Return_ViewResult_With_Offers()
        {
            // Arrange
            var offers = new List<ResultOfferDto>
            {
                new ResultOfferDto { Title = "Offer 1", ImageUrl = "img1.jpg" }
            };

            _mockService.Setup(x => x.GetAllOfferAsync()).ReturnsAsync(offers);

            // Act
            var result = await _controller.OfferList();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ResultOfferDto>>(viewResult.Model);
            Assert.Single(model);
        }

        #endregion

        #region UpdateOffer Tests

        [Fact]
        public async Task UpdateOffer_POST_Should_Call_Service_UpdateOfferAsync()
        {
            // Arrange
            var updateDto = new UpdateOfferDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated Offer",
                ImageUrl = "updated.jpg"
            };

            _mockService.Setup(x => x.UpdateOfferAsync(It.IsAny<UpdateOfferDto>())).Returns(Task.CompletedTask);

            // Act
            await _controller.UpdateOffer(updateDto);

            // Assert
            _mockService.Verify(x => x.UpdateOfferAsync(It.IsAny<UpdateOfferDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOffer_POST_Should_Return_RedirectToAction()
        {
            // Arrange
            var updateDto = new UpdateOfferDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated Offer",
                ImageUrl = "updated.jpg"
            };

            _mockService.Setup(x => x.UpdateOfferAsync(It.IsAny<UpdateOfferDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateOffer(updateDto);

            // Assert
            var resultRedirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("OfferList", resultRedirect.ActionName);
        }

        [Fact]
        public async Task UpdateOffer_GET_Should_Call_Service_GetOfferByIdAsync()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            var offerDto = new GetOfferByIdDto { Title = "Test Offer" };

            _mockService.Setup(x => x.GetOfferByIdAsync(id)).ReturnsAsync(offerDto);

            // Act
            var result = await _controller.UpdateOffer(id);

            // Assert
            _mockService.Verify(x => x.GetOfferByIdAsync(id), Times.Once);
        }

        #endregion
    }
}