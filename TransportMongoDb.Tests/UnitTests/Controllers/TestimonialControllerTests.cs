using DatabaseMastery.TransportMongoDb.Controllers;
using DatabaseMastery.TransportMongoDb.Dtos.TestimonialDtos;
using DatabaseMastery.TransportMongoDb.Services.TestimonialServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TransportMongoDb.Tests.UnitTests.Controllers
{
    public class TestimonialControllerTests
    {
        private readonly Mock<ITestimonialService> _mockService;
        private readonly TestimonialController _controller;

        public TestimonialControllerTests()
        {
            _mockService = new Mock<ITestimonialService>();
            _controller = new TestimonialController(_mockService.Object);
        }

        #region CREATE

        [Fact]
        public async Task CreateTestimonial_Should_Call_Service_Once()
        {
            var dto = new CreateTestimonialDto
            {
                NameSurname = "John Doe",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateTestimonialAsync(It.IsAny<CreateTestimonialDto>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.CreateTestimonial(dto);

            Assert.IsType<RedirectToActionResult>(result);

            _mockService.Verify(x => x.CreateTestimonialAsync(It.IsAny<CreateTestimonialDto>()), Times.Once);
        }

        [Fact]
        public async Task CreateTestimonial_Should_Return_Redirect()
        {
            var dto = new CreateTestimonialDto
            {
                NameSurname = "John Doe",
                ImageUrl = "test.jpg"
            };

            _mockService.Setup(x => x.CreateTestimonialAsync(It.IsAny<CreateTestimonialDto>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.CreateTestimonial(dto);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("TestimonialList", redirect.ActionName);
        }

        [Fact]
        public async Task CreateTestimonial_Should_Send_Correct_Data()
        {
            var dto = new CreateTestimonialDto
            {
                NameSurname = "Jane",
                Title = "CEO",
                ReviewDetail = "Great",
                ImageUrl = "img.jpg",
                ReviewScore = 5,
                Status = true
            };

            _mockService.Setup(x => x.CreateTestimonialAsync(It.IsAny<CreateTestimonialDto>()))
                        .Returns(Task.CompletedTask);

            await _controller.CreateTestimonial(dto);

            _mockService.Verify(x => x.CreateTestimonialAsync(It.Is<CreateTestimonialDto>(x =>
                x.NameSurname == "Jane" &&
                x.Title == "CEO" &&
                x.ReviewDetail == "Great" &&
                x.ImageUrl == "img.jpg" &&
                x.ReviewScore == 5 &&
                x.Status == true
            )), Times.Once);
        }

        #endregion

        #region DELETE

        [Fact]
        public async Task DeleteTestimonial_Should_Call_Service()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteTestimonialAsync(id))
                        .Returns(Task.CompletedTask);

            await _controller.DeleteTestimonial(id);

            _mockService.Verify(x => x.DeleteTestimonialAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteTestimonial_Should_Redirect()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteTestimonialAsync(id))
                        .Returns(Task.CompletedTask);

            var result = await _controller.DeleteTestimonial(id);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("TestimonialList", redirect.ActionName);
        }

        #endregion

        #region LIST

        [Fact]
        public async Task TestimonialList_Should_Call_Service()
        {
            var list = new List<ResultTestimonialDto>
            {
                new ResultTestimonialDto { NameSurname = "A" },
                new ResultTestimonialDto { NameSurname = "B" }
            };

            _mockService.Setup(x => x.GetAllTestimonialAsync())
                        .ReturnsAsync(list);

            await _controller.TestimonialList();

            _mockService.Verify(x => x.GetAllTestimonialAsync(), Times.Once);
        }

        [Fact]
        public async Task TestimonialList_Should_Return_View()
        {
            var list = new List<ResultTestimonialDto>
            {
                new ResultTestimonialDto { NameSurname = "A" }
            };

            _mockService.Setup(x => x.GetAllTestimonialAsync())
                        .ReturnsAsync(list);

            var result = await _controller.TestimonialList();

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ResultTestimonialDto>>(view.Model);

            Assert.Single(model);
        }

        #endregion

        #region UPDATE

        [Fact]
        public async Task UpdateTestimonial_POST_Should_Call_Service()
        {
            var dto = new UpdateTestimonialDto
            {
                Id = "507f1f77bcf86cd799439011",
                NameSurname = "Updated",
                ImageUrl = "updated.jpg"
            };

            _mockService.Setup(x => x.UpdateTestimonialAsync(It.IsAny<UpdateTestimonialDto>()))
                        .Returns(Task.CompletedTask);

            await _controller.UpdateTestimonial(dto);

            _mockService.Verify(x => x.UpdateTestimonialAsync(It.IsAny<UpdateTestimonialDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTestimonial_POST_Should_Redirect()
        {
            var dto = new UpdateTestimonialDto
            {
                Id = "507f1f77bcf86cd799439011",
                NameSurname = "Updated",
                ImageUrl = "updated.jpg"
            };

            _mockService.Setup(x => x.UpdateTestimonialAsync(It.IsAny<UpdateTestimonialDto>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.UpdateTestimonial(dto);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("TestimonialList", redirect.ActionName);
        }

        [Fact]
        public async Task UpdateTestimonial_GET_Should_Call_Service()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.GetTestimonialByIdAsync(id))
                        .ReturnsAsync(new GetTestimonialByIdDto());

            await _controller.UpdateTestimonial(id);

            _mockService.Verify(x => x.GetTestimonialByIdAsync(id), Times.Once);
        }

        #endregion
    }
}