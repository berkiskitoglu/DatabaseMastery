using DatabaseMastery.TransportMongoDb.Controllers;
using DatabaseMastery.TransportMongoDb.Dtos.SliderDtos;
using DatabaseMastery.TransportMongoDb.Services.SliderServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransportMongoDb.Tests.UnitTests.Controllers
{
    public class SliderControllerTests
    {
        private readonly Mock<ISliderService> _mockService;
        private readonly SliderController _controller;

        public SliderControllerTests()
        {
            _mockService = new Mock<ISliderService>();
            _controller = new SliderController(_mockService.Object);
        }

        #region CreateSlider (POST) Tests

        [Fact]
        public async Task CreateSlider_Should_Call_Service_CreateSliderAsync_Once()
        {
            // Arrange
            var createDto = new CreateSliderDto
            {
                Title = "Test",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateSliderAsync(It.IsAny<CreateSliderDto>())).Returns(Task.CompletedTask);

            // Action
            var result = await _controller.CreateSlider(createDto);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);

            _mockService.Verify(x => x.CreateSliderAsync(It.IsAny<CreateSliderDto>()), Times.Once);
        }

        [Fact]
        public async Task CreateSlider_Should_Return_RedirectToAction()
        {
            // Arrange
            var createDto = new CreateSliderDto
            {
                Title = "Test",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateSliderAsync(It.IsAny<CreateSliderDto>())).Returns(Task.CompletedTask);

            // Act

            var result = await _controller.CreateSlider(createDto);

            // Assert

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("SliderList", redirectResult.ActionName);
        }

        [Fact]
        public async Task CreateSlider_Should_Pass_Correct_Dto_To_Service()
        {
            // Arrange
            var createDto = new CreateSliderDto
            {
                Title = "Test Slider",
                SubTitle = "Test SubTitle",
                Description = "Test Description",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateSliderAsync(It.IsAny<CreateSliderDto>())).Returns(Task.CompletedTask);

            // Act
             await _controller.CreateSlider(createDto);

            // Assert
            _mockService.Verify(x => x.CreateSliderAsync(It.Is<CreateSliderDto>(dto =>
                dto.Title == "Test Slider" &&
                dto.SubTitle == "Test SubTitle" &&
                dto.Description == "Test Description" &&
                dto.ImageUrl == "test.jpg"
            )), Times.Once());

        }

        [Fact]
        public async Task CreateSlider_Should_Thrown_When_Service_Fails()
        {
            // Arrange
            var createDto = new CreateSliderDto
            {
                Title = "Test",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateSliderAsync(It.IsAny<CreateSliderDto>())).ThrowsAsync(new Exception("Service Error"));

            // Act & Assert

            var exception = await Assert.ThrowsAsync<Exception>(
                () => _controller.CreateSlider(createDto)
             );

            Assert.Equal("Service Error", exception.Message);
        }


        #endregion

        #region DeleteSlider Tests

        [Fact]
        public async Task DeleteSlider_Should_Call_Service_DeleteSliderAsync()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteSliderAsync(id)).Returns(Task.CompletedTask);

            // Act
            await _controller.DeleteSlider(id);

            // Assert
            _mockService.Verify(x => x.DeleteSliderAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteSlider_Should_Return_Redirect_To_Action()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteSliderAsync(id)).Returns(Task.CompletedTask);

            // Act

            var result = await _controller.DeleteSlider(id);

            // Assert

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("SliderList", redirectResult.ActionName);

        }

        #endregion

        #region SliderList Tests

        [Fact]
        public async Task SliderList_Should_Call_Service_GetAllSliderAsync()
        {
            // Arrange
            var sliders = new List<ResultSliderDto>
            {
                new ResultSliderDto {Title = "Slider 1"},
                new ResultSliderDto {Title = "Slider 2"}
            };

            _mockService.Setup(x => x.GetAllSliderAsync()).ReturnsAsync(sliders);

            // Act
            var result = await _controller.SliderList();

            // Assert
            _mockService.Verify(x => x.GetAllSliderAsync(), Times.Once);
        }

        [Fact]
        public async Task SliderList_Should_Return_ViewResult_With_Sliders()
        {
            // Arrange
            var sliders = new List<ResultSliderDto>
            {
                new ResultSliderDto { Title = "Slider 1" }
            };

            _mockService.Setup(x => x.GetAllSliderAsync()).ReturnsAsync(sliders);

            // Act
            var result = await _controller.SliderList();

            // Assert

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ResultSliderDto>>(viewResult.Model);
            Assert.Single(model);
        }

        #endregion

        #region UpdateSlider Tests

        [Fact]
        public async Task UpdateSlider_POST_Should_Call_Service_UpdateSliderAsync()
        {
            // Arrange
            var updateDto = new UpdateSliderDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "UpdatedSlider",
                ImageUrl = "updatedSlider.jpg"
            };

            _mockService.Setup(x => x.UpdateSliderAsync(It.IsAny<UpdateSliderDto>())).Returns(Task.CompletedTask);

            // Act
            await _controller.UpdateSlider(updateDto);

            // Arrange
            _mockService.Verify(x => x.UpdateSliderAsync(It.IsAny<UpdateSliderDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSlider_POST_Should_Return_RedirectToAction()
        {
            // Arrange
            var updateDto = new UpdateSliderDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "UpdatedSlider",
                ImageUrl = "updatedSlider.jpg"
            };

            _mockService.Setup(x => x.UpdateSliderAsync(It.IsAny<UpdateSliderDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateSlider(updateDto);

            // Assert

            var resultRedirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("SliderList", resultRedirect.ActionName);

        }

        [Fact]
        public async Task UpdateSlider_GET_Should_Call_Service_GetSliderByIdAsync()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            var sliderDto = new GetSliderByIdDto { Title = "Test" };

            _mockService.Setup(x => x.GetSliderByIdAsync(id)).ReturnsAsync(sliderDto);

            // Action
            var result = await _controller.UpdateSlider(id);

            // Assert

            _mockService.Verify(x=>x.GetSliderByIdAsync(id),Times.Once);
        }

        #endregion



    }
}
