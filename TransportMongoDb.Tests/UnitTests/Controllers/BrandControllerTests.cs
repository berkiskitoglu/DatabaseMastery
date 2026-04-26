using DatabaseMastery.TransportMongoDb.Controllers;
using DatabaseMastery.TransportMongoDb.Dtos.BrandDtos;
using DatabaseMastery.TransportMongoDb.Services.BrandServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransportMongoDb.Tests.UnitTests.Controllers
{
    public class BrandControllerTests
    {
        private readonly Mock<IBrandService> _mockService;
        private readonly BrandController _controller;

        public BrandControllerTests()
        {
            _mockService = new Mock<IBrandService>();
            _controller = new BrandController(_mockService.Object);
        }

        #region CreateBrand (POST) Tests

        [Fact]
        public async Task CreateBrand_Should_Call_Service_CreateBrandAsync_Once()
        {
            // Arrange
            var createDto = new CreateBrandDto
            {
                BrandName = "Test Brand",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateBrandAsync(It.IsAny<CreateBrandDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateBrand(createDto);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            _mockService.Verify(x => x.CreateBrandAsync(It.IsAny<CreateBrandDto>()), Times.Once);
        }

        [Fact]
        public async Task CreateBrand_Should_Return_RedirectToAction()
        {
            // Arrange
            var createDto = new CreateBrandDto
            {
                BrandName = "Test Brand",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateBrandAsync(It.IsAny<CreateBrandDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateBrand(createDto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("BrandList", redirectResult.ActionName);
        }

        [Fact]
        public async Task CreateBrand_Should_Pass_Correct_Dto_To_Service()
        {
            // Arrange
            var createDto = new CreateBrandDto
            {
                BrandName = "Test Brand",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateBrandAsync(It.IsAny<CreateBrandDto>())).Returns(Task.CompletedTask);

            // Act
            await _controller.CreateBrand(createDto);

            // Assert
            _mockService.Verify(x => x.CreateBrandAsync(It.Is<CreateBrandDto>(dto =>
                dto.BrandName == "Test Brand" &&
                dto.ImageUrl == "test.jpg"
            )), Times.Once());
        }

        [Fact]
        public async Task CreateBrand_Should_Throw_When_Service_Fails()
        {
            // Arrange
            var createDto = new CreateBrandDto
            {
                BrandName = "Test Brand",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateBrandAsync(It.IsAny<CreateBrandDto>())).ThrowsAsync(new Exception("Service Error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _controller.CreateBrand(createDto)
            );

            Assert.Equal("Service Error", exception.Message);
        }

        #endregion

        #region DeleteBrand Tests

        [Fact]
        public async Task DeleteBrand_Should_Call_Service_DeleteBrandAsync()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteBrandAsync(id)).Returns(Task.CompletedTask);

            // Act
            await _controller.DeleteBrand(id);

            // Assert
            _mockService.Verify(x => x.DeleteBrandAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteBrand_Should_Return_Redirect_To_Action()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteBrandAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteBrand(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("BrandList", redirectResult.ActionName);
        }

        #endregion

        #region BrandList Tests

        [Fact]
        public async Task BrandList_Should_Call_Service_GetAllBrandAsync()
        {
            // Arrange
            var brands = new List<ResultBrandDto>
            {
                new ResultBrandDto { BrandName = "Brand 1", ImageUrl = "img1.jpg" },
                new ResultBrandDto { BrandName = "Brand 2", ImageUrl = "img2.jpg" }
            };

            _mockService.Setup(x => x.GetAllBrandAsync()).ReturnsAsync(brands);

            // Act
            var result = await _controller.BrandList();

            // Assert
            _mockService.Verify(x => x.GetAllBrandAsync(), Times.Once);
        }

        [Fact]
        public async Task BrandList_Should_Return_ViewResult_With_Brands()
        {
            // Arrange
            var brands = new List<ResultBrandDto>
            {
                new ResultBrandDto { BrandName = "Brand 1", ImageUrl = "img1.jpg" }
            };

            _mockService.Setup(x => x.GetAllBrandAsync()).ReturnsAsync(brands);

            // Act
            var result = await _controller.BrandList();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ResultBrandDto>>(viewResult.Model);
            Assert.Single(model);
        }

        #endregion

        #region UpdateBrand Tests

        [Fact]
        public async Task UpdateBrand_POST_Should_Call_Service_UpdateBrandAsync()
        {
            // Arrange
            var updateDto = new UpdateBrandDto
            {
                Id = "507f1f77bcf86cd799439011",
                BrandName = "Updated Brand",
                ImageUrl = "updated.jpg"
            };

            _mockService.Setup(x => x.UpdateBrandAsync(It.IsAny<UpdateBrandDto>())).Returns(Task.CompletedTask);

            // Act
            await _controller.UpdateBrand(updateDto);

            // Assert
            _mockService.Verify(x => x.UpdateBrandAsync(It.IsAny<UpdateBrandDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateBrand_POST_Should_Return_RedirectToAction()
        {
            // Arrange
            var updateDto = new UpdateBrandDto
            {
                Id = "507f1f77bcf86cd799439011",
                BrandName = "Updated Brand",
                ImageUrl = "updated.jpg"
            };

            _mockService.Setup(x => x.UpdateBrandAsync(It.IsAny<UpdateBrandDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateBrand(updateDto);

            // Assert
            var resultRedirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("BrandList", resultRedirect.ActionName);
        }

        [Fact]
        public async Task UpdateBrand_GET_Should_Call_Service_GetBrandByIdAsync()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            var brandDto = new GetBrandByIdDto { BrandName = "Test Brand", ImageUrl = "test.jpg" };

            _mockService.Setup(x => x.GetBrandByIdAsync(id)).ReturnsAsync(brandDto);

            // Act
            var result = await _controller.UpdateBrand(id);

            // Assert
            _mockService.Verify(x => x.GetBrandByIdAsync(id), Times.Once);
        }

        #endregion
    }
}