using DatabaseMastery.TransportMongoDb.Controllers;
using DatabaseMastery.TransportMongoDb.Dtos.ProjectSectionDtos;
using DatabaseMastery.TransportMongoDb.Services.ProjectSectionServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TransportMongoDb.Tests.UnitTests.Controllers
{
    public class ProjectSectionControllerTests
    {
        private readonly Mock<IProjectSectionService> _mockService;
        private readonly ProjectSectionController _controller;

        public ProjectSectionControllerTests()
        {
            _mockService = new Mock<IProjectSectionService>();
            _controller = new ProjectSectionController(_mockService.Object);
        }

        #region Create

        [Fact]
        public async Task Create_Should_Call_Service()
        {
            var dto = new CreateProjectSectionDto
            {
                Title = "Test",
                ImageUrl = "img.jpg"
            };

            _mockService.Setup(x => x.CreateProjectSectionAsync(It.IsAny<CreateProjectSectionDto>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.CreateProjectSection(dto);

            Assert.IsType<RedirectToActionResult>(result);
            _mockService.Verify(x => x.CreateProjectSectionAsync(It.IsAny<CreateProjectSectionDto>()), Times.Once);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_Should_Call_Service()
        {
            var id = "1";

            _mockService.Setup(x => x.DeleteProjectSectionAsync(id))
                .Returns(Task.CompletedTask);

            await _controller.DeleteProjectSection(id);

            _mockService.Verify(x => x.DeleteProjectSectionAsync(id), Times.Once);
        }

        #endregion

        #region List

        [Fact]
        public async Task List_Should_Return_View()
        {
            var list = new List<ResultProjectSectionDto>
            {
                new ResultProjectSectionDto { Title = "A" }
            };

            _mockService.Setup(x => x.GetAllProjectSectionAsync()).ReturnsAsync(list);

            var result = await _controller.ProjectSectionList();

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ResultProjectSectionDto>>(view.Model);

            Assert.Single(model);
        }

        #endregion

        #region Update POST

        [Fact]
        public async Task Update_POST_Should_Call_Service()
        {
            var dto = new UpdateProjectSectionDto
            {
                Id = "1",
                Title = "Updated"
            };

            _mockService.Setup(x => x.UpdateProjectSectionAsync(It.IsAny<UpdateProjectSectionDto>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateProjectSection(dto);

            Assert.IsType<RedirectToActionResult>(result);
            _mockService.Verify(x => x.UpdateProjectSectionAsync(It.IsAny<UpdateProjectSectionDto>()), Times.Once);
        }

        #endregion

        #region Update GET

        [Fact]
        public async Task Update_GET_Should_Call_Service()
        {
            var id = "1";

            _mockService.Setup(x => x.GetProjectSectionByIdAsync(id))
                .ReturnsAsync(new GetProjectSectionByIdDto());

            await _controller.UpdateProjectSection(id);

            _mockService.Verify(x => x.GetProjectSectionByIdAsync(id), Times.Once);
        }

        #endregion
    }
}