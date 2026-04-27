using DatabaseMastery.TransportMongoDb.Controllers;
using DatabaseMastery.TransportMongoDb.Dtos.AboutDtos;
using DatabaseMastery.TransportMongoDb.Services.AboutServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TransportMongoDb.Tests.UnitTests.Controllers
{
    public class AboutControllerTests
    {
        private readonly Mock<IAboutService> _mockService;
        private readonly AboutController _controller;

        public AboutControllerTests()
        {
            _mockService = new Mock<IAboutService>();
            _controller = new AboutController(_mockService.Object);
        }

        #region CreateAbout (POST) Tests

        [Fact]
        public async Task CreateAbout_Should_Call_Service_CreateAboutAsync_Once()
        {
            var createDto = new CreateAboutDto
            {
                Title = "Test",
                Description = "Desc",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateAboutAsync(It.IsAny<CreateAboutDto>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.CreateAbout(createDto);

            Assert.IsType<RedirectToActionResult>(result);

            _mockService.Verify(x => x.CreateAboutAsync(It.IsAny<CreateAboutDto>()), Times.Once);
        }

        [Fact]
        public async Task CreateAbout_Should_Return_RedirectToAction()
        {
            var createDto = new CreateAboutDto
            {
                Title = "Test",
                Description = "Desc",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateAboutAsync(It.IsAny<CreateAboutDto>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.CreateAbout(createDto);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("AboutList", redirectResult.ActionName);
        }

        [Fact]
        public async Task CreateAbout_Should_Pass_Correct_Dto_To_Service()
        {
            var createDto = new CreateAboutDto
            {
                Title = "About Title",
                Description = "About Description",
                ImageUrl = "about.jpg"
            };

            _mockService.Setup(x => x.CreateAboutAsync(It.IsAny<CreateAboutDto>()))
                        .Returns(Task.CompletedTask);

            await _controller.CreateAbout(createDto);

            _mockService.Verify(x => x.CreateAboutAsync(It.Is<CreateAboutDto>(dto =>
                dto.Title == "About Title" &&
                dto.Description == "About Description" &&
                dto.ImageUrl == "about.jpg"
            )), Times.Once());
        }

        [Fact]
        public async Task CreateAbout_Should_Throw_When_Service_Fails()
        {
            var createDto = new CreateAboutDto
            {
                Title = "Test",
                Description = "Desc",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateAboutAsync(It.IsAny<CreateAboutDto>()))
                        .ThrowsAsync(new Exception("Service Error"));

            var exception = await Assert.ThrowsAsync<Exception>(
                () => _controller.CreateAbout(createDto)
            );

            Assert.Equal("Service Error", exception.Message);
        }

        #endregion

        #region DeleteAbout Tests

        [Fact]
        public async Task DeleteAbout_Should_Call_Service_DeleteAboutAsync()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteAboutAsync(id))
                        .Returns(Task.CompletedTask);

            await _controller.DeleteAbout(id);

            _mockService.Verify(x => x.DeleteAboutAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAbout_Should_Return_Redirect_To_Action()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteAboutAsync(id))
                        .Returns(Task.CompletedTask);

            var result = await _controller.DeleteAbout(id);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("AboutList", redirectResult.ActionName);
        }

        #endregion

        #region AboutList Tests

        [Fact]
        public async Task AboutList_Should_Call_Service_GetAllAboutAsync()
        {
            var abouts = new List<ResultAboutDto>
            {
                new ResultAboutDto { Title = "About 1" },
                new ResultAboutDto { Title = "About 2" }
            };

            _mockService.Setup(x => x.GetAllAboutAsync())
                        .ReturnsAsync(abouts);

            var result = await _controller.AboutList();

            _mockService.Verify(x => x.GetAllAboutAsync(), Times.Once);
        }

        [Fact]
        public async Task AboutList_Should_Return_ViewResult_With_Abouts()
        {
            var abouts = new List<ResultAboutDto>
            {
                new ResultAboutDto { Title = "About 1" }
            };

            _mockService.Setup(x => x.GetAllAboutAsync())
                        .ReturnsAsync(abouts);

            var result = await _controller.AboutList();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ResultAboutDto>>(viewResult.Model);
            Assert.Single(model);
        }

        #endregion

        #region UpdateAbout Tests

        [Fact]
        public async Task UpdateAbout_POST_Should_Call_Service_UpdateAboutAsync()
        {
            var updateDto = new UpdateAboutDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated About",
                Description = "Updated Desc",
                ImageUrl = "updated.jpg"
            };

            _mockService.Setup(x => x.UpdateAboutAsync(It.IsAny<UpdateAboutDto>()))
                        .Returns(Task.CompletedTask);

            await _controller.UpdateAbout(updateDto);

            _mockService.Verify(x => x.UpdateAboutAsync(It.IsAny<UpdateAboutDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAbout_POST_Should_Return_RedirectToAction()
        {
            var updateDto = new UpdateAboutDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated About",
                Description = "Updated Desc",
                ImageUrl = "updated.jpg"
            };

            _mockService.Setup(x => x.UpdateAboutAsync(It.IsAny<UpdateAboutDto>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.UpdateAbout(updateDto);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("AboutList", redirectResult.ActionName);
        }

        [Fact]
        public async Task UpdateAbout_GET_Should_Call_Service_GetAboutByIdAsync()
        {
            var id = "507f1f77bcf86cd799439011";
            var aboutDto = new GetAboutByIdDto { Title = "Test" };

            _mockService.Setup(x => x.GetAboutByIdAsync(id))
                        .ReturnsAsync(aboutDto);

            var result = await _controller.UpdateAbout(id);

            _mockService.Verify(x => x.GetAboutByIdAsync(id), Times.Once);
        }

        #endregion
    }
}