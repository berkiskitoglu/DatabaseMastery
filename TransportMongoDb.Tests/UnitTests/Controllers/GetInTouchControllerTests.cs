using DatabaseMastery.TransportMongoDb.Controllers;
using DatabaseMastery.TransportMongoDb.Dtos.GetInTouchDtos;
using DatabaseMastery.TransportMongoDb.Services.GetInTouchServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TransportMongoDb.Tests.UnitTests.Controllers
{
    public class GetInTouchControllerTests
    {
        private readonly Mock<IGetInTouchService> _mockService;
        private readonly GetInTouchController _controller;

        public GetInTouchControllerTests()
        {
            _mockService = new Mock<IGetInTouchService>();
            _controller = new GetInTouchController(_mockService.Object);
        }

        #region CreateGetInTouch (POST)

        [Fact]
        public async Task CreateGetInTouch_Should_Call_Service_Once()
        {
            var dto = new CreateGetInTouchDto
            {
                MainTitle = "Test",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateGetInTouchAsync(It.IsAny<CreateGetInTouchDto>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.CreateGetInTouch(dto);

            Assert.IsType<RedirectToActionResult>(result);

            _mockService.Verify(x => x.CreateGetInTouchAsync(It.IsAny<CreateGetInTouchDto>()), Times.Once);
        }

        [Fact]
        public async Task CreateGetInTouch_Should_Return_Redirect()
        {
            var dto = new CreateGetInTouchDto();

            _mockService.Setup(x => x.CreateGetInTouchAsync(It.IsAny<CreateGetInTouchDto>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.CreateGetInTouch(dto);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("GetInTouchList", redirect.ActionName);
        }

        [Fact]
        public async Task CreateGetInTouch_Should_Pass_Correct_Data()
        {
            var dto = new CreateGetInTouchDto
            {
                MainTitle = "Main",
                Description = "Desc",
                ImageUrl = "img.jpg"
            };

            _mockService.Setup(x => x.CreateGetInTouchAsync(It.IsAny<CreateGetInTouchDto>()))
                .Returns(Task.CompletedTask);

            await _controller.CreateGetInTouch(dto);

            _mockService.Verify(x => x.CreateGetInTouchAsync(It.Is<CreateGetInTouchDto>(x =>
                x.MainTitle == "Main" &&
                x.Description == "Desc" &&
                x.ImageUrl == "img.jpg"
            )), Times.Once);
        }

        [Fact]
        public async Task CreateGetInTouch_Should_Throw_When_Service_Fails()
        {
            _mockService.Setup(x => x.CreateGetInTouchAsync(It.IsAny<CreateGetInTouchDto>()))
                .ThrowsAsync(new Exception("Service Error"));

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                _controller.CreateGetInTouch(new CreateGetInTouchDto())
            );

            Assert.Equal("Service Error", ex.Message);
        }

        #endregion

        #region DeleteGetInTouch

        [Fact]
        public async Task DeleteGetInTouch_Should_Call_Service()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteGetInTouchAsync(id))
                .Returns(Task.CompletedTask);

            await _controller.DeleteGetInTouch(id);

            _mockService.Verify(x => x.DeleteGetInTouchAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteGetInTouch_Should_Return_Redirect()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteGetInTouchAsync(id))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteGetInTouch(id);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("GetInTouchList", redirect.ActionName);
        }

        #endregion

        #region GetInTouchList

        [Fact]
        public async Task GetInTouchList_Should_Call_Service()
        {
            var list = new List<ResultGetInTouchDto>
            {
                new ResultGetInTouchDto { MainTitle = "A" },
                new ResultGetInTouchDto { MainTitle = "B" }
            };

            _mockService.Setup(x => x.GetAllGetInTouchAsync()).ReturnsAsync(list);

            await _controller.GetInTouchList();

            _mockService.Verify(x => x.GetAllGetInTouchAsync(), Times.Once);
        }

        [Fact]
        public async Task GetInTouchList_Should_Return_View_With_Data()
        {
            var list = new List<ResultGetInTouchDto>
            {
                new ResultGetInTouchDto { MainTitle = "Test" }
            };

            _mockService.Setup(x => x.GetAllGetInTouchAsync()).ReturnsAsync(list);

            var result = await _controller.GetInTouchList();

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ResultGetInTouchDto>>(view.Model);

            Assert.Single(model);
        }

        #endregion

        #region UpdateGetInTouch

        [Fact]
        public async Task UpdateGetInTouch_POST_Should_Call_Service()
        {
            var dto = new UpdateGetInTouchDto
            {
                Id = "507f1f77bcf86cd799439011",
                MainTitle = "Updated"
            };

            _mockService.Setup(x => x.UpdateGetInTouchAsync(It.IsAny<UpdateGetInTouchDto>()))
                .Returns(Task.CompletedTask);

            await _controller.UpdateGetInTouch(dto);

            _mockService.Verify(x => x.UpdateGetInTouchAsync(It.IsAny<UpdateGetInTouchDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateGetInTouch_POST_Should_Return_Redirect()
        {
            var dto = new UpdateGetInTouchDto();

            _mockService.Setup(x => x.UpdateGetInTouchAsync(It.IsAny<UpdateGetInTouchDto>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateGetInTouch(dto);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("GetInTouchList", redirect.ActionName);
        }

        [Fact]
        public async Task UpdateGetInTouch_GET_Should_Call_Service()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.GetInTouchByIdAsync(id))
                .ReturnsAsync(new GetInTouchByIdDto());

            await _controller.UpdateGetInTouch(id);

            _mockService.Verify(x => x.GetInTouchByIdAsync(id), Times.Once);
        }

        #endregion
    }
}