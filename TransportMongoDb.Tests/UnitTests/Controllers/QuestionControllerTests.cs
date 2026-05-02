using DatabaseMastery.TransportMongoDb.Controllers;
using DatabaseMastery.TransportMongoDb.Dtos.QuestionDtos;
using DatabaseMastery.TransportMongoDb.Services.QuestionServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TransportMongoDb.Tests.UnitTests.Controllers
{
    public class QuestionControllerTests
    {
        private readonly Mock<IQuestionService> _mockService;
        private readonly QuestionController _controller;

        public QuestionControllerTests()
        {
            _mockService = new Mock<IQuestionService>();
            _controller = new QuestionController(_mockService.Object);
        }

        #region Create

        [Fact]
        public async Task CreateQuestion_Should_Call_Service()
        {
            var dto = new CreateQuestionDto
            {
                Title = "Test",
                Description = "Desc",
                Status = true
            };

            _mockService.Setup(x => x.CreateQuestionAsync(It.IsAny<CreateQuestionDto>()))
                        .Returns(Task.CompletedTask);

            var result = await _controller.CreateQuestion(dto);

            Assert.IsType<RedirectToActionResult>(result);
            _mockService.Verify(x => x.CreateQuestionAsync(It.IsAny<CreateQuestionDto>()), Times.Once);
        }

        #endregion

        #region List

        [Fact]
        public async Task QuestionList_Should_Return_View()
        {
            _mockService.Setup(x => x.GetAllQuestionAsync())
                .ReturnsAsync(new List<ResultQuestionDto> { new ResultQuestionDto() });

            var result = await _controller.QuestionList();

            var view = Assert.IsType<ViewResult>(result);
            Assert.NotNull(view.Model);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteQuestion_Should_Call_Service()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.DeleteQuestionAsync(id)).Returns(Task.CompletedTask);

            await _controller.DeleteQuestion(id);

            _mockService.Verify(x => x.DeleteQuestionAsync(id), Times.Once);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateQuestion_POST_Should_Call_Service()
        {
            var dto = new UpdateQuestionDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated",
                Description = "Desc",
                Status = false
            };

            _mockService.Setup(x => x.UpdateQuestionAsync(It.IsAny<UpdateQuestionDto>()))
                        .Returns(Task.CompletedTask);

            await _controller.UpdateQuestion(dto);

            _mockService.Verify(x => x.UpdateQuestionAsync(It.IsAny<UpdateQuestionDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateQuestion_GET_Should_Call_Service()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockService.Setup(x => x.GetQuestionByIdAsync(id))
                        .ReturnsAsync(new GetQuestionByIdDto());

            await _controller.UpdateQuestion(id);

            _mockService.Verify(x => x.GetQuestionByIdAsync(id), Times.Once);
        }

        #endregion
    }
}