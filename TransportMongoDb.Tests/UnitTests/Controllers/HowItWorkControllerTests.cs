using DatabaseMastery.TransportMongoDb.Controllers;
using DatabaseMastery.TransportMongoDb.Dtos.HowItWorkDtos;
using DatabaseMastery.TransportMongoDb.Services.HowItWorkServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TransportMongoDb.Tests.UnitTests.Controllers
{
    public class HowItWorkControllerTests
    {
        private readonly Mock<IHowItWorkService> _mockService;
        private readonly HowItWorkController _controller;

        public HowItWorkControllerTests()
        {
            _mockService = new Mock<IHowItWorkService>();
            _controller = new HowItWorkController(_mockService.Object);
        }

        #region CREATE

        [Fact]
        public async Task CreateHowItWork_Should_Call_Service()
        {
            var dto = new CreateHowItWorkDto
            {
                Title = "Test",
                Description = "Desc",
                IconUrl = "icon.png",
                Status = true
            };

            _mockService.Setup(x => x.CreateHowItWorkAsync(It.IsAny<CreateHowItWorkDto>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.CreateHowItWork(dto);

            Assert.IsType<RedirectToActionResult>(result);

            _mockService.Verify(x => x.CreateHowItWorkAsync(It.IsAny<CreateHowItWorkDto>()), Times.Once);
        }

        #endregion

        #region DELETE

        [Fact]
        public async Task DeleteHowItWork_Should_Call_Service()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteHowItWorkAsync(id))
                        .Returns(Task.CompletedTask);

            await _controller.DeleteHowItWork(id);

            _mockService.Verify(x => x.DeleteHowItWorkAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteHowItWork_Should_Redirect()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteHowItWorkAsync(id))
                        .Returns(Task.CompletedTask);

            var result = await _controller.DeleteHowItWork(id);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HowItWorkList", redirect.ActionName);
        }

        #endregion

        #region LIST

        [Fact]
        public async Task HowItWorkList_Should_Return_View()
        {
            var list = new List<ResultHowItWorkDto>
            {
                new ResultHowItWorkDto { Title = "Step 1" }
            };

            _mockService.Setup(x => x.GetAllHowItWorkAsync())
                        .ReturnsAsync(list);

            var result = await _controller.HowItWorkList();

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ResultHowItWorkDto>>(view.Model);

            Assert.Single(model);
        }

        #endregion

        #region UPDATE

        [Fact]
        public async Task UpdateHowItWork_POST_Should_Call_Service()
        {
            var dto = new UpdateHowItWorkDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated"
            };

            _mockService.Setup(x => x.UpdateHowItWorkAsync(It.IsAny<UpdateHowItWorkDto>()))
                        .Returns(Task.CompletedTask);

            await _controller.UpdateHowItWork(dto);

            _mockService.Verify(x => x.UpdateHowItWorkAsync(It.IsAny<UpdateHowItWorkDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateHowItWork_GET_Should_Call_Service()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.GetHowItWorkByIdAsync(id))
                        .ReturnsAsync(new GetHowItWorkByIdDto());

            await _controller.UpdateHowItWork(id);

            _mockService.Verify(x => x.GetHowItWorkByIdAsync(id), Times.Once);
        }

        #endregion
    }
}