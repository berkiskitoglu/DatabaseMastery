using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.ProjectSectionDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.ProjectSectionServices;
using Moq;

namespace TransportMongoDb.Tests.UnitTests.Services
{
    public class ProjectSectionServiceTests
    {
        private readonly Mock<IGenericRepository<ProjectSection>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProjectSectionService _service;

        public ProjectSectionServiceTests()
        {
            _mockRepository = new Mock<IGenericRepository<ProjectSection>>();
            _mockMapper = new Mock<IMapper>();
            _service = new ProjectSectionService(_mockRepository.Object, _mockMapper.Object);
        }

        #region Create

        [Fact]
        public async Task CreateProjectSectionAsync_Should_Call_Repository()
        {
            var dto = new CreateProjectSectionDto
            {
                Title = "Test",
                Description = "Desc",
                ImageUrl = "img.jpg",
                IsStatus = true
            };

            _mockMapper.Setup(x => x.Map<ProjectSection>(dto))
                .Returns(new ProjectSection { Title = dto.Title });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<ProjectSection>()))
                .Returns(Task.CompletedTask);

            await _service.CreateProjectSectionAsync(dto);

            _mockRepository.Verify(x => x.CreateAsync(It.IsAny<ProjectSection>()), Times.Once);
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAllProjectSectionAsync_Should_Return_List()
        {
            var list = new List<ProjectSection>
            {
                new ProjectSection { Title = "A" }
            };

            var mapped = new List<ResultProjectSectionDto>
            {
                new ResultProjectSectionDto { Title = "A" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(list);
            _mockMapper.Setup(x => x.Map<List<ResultProjectSectionDto>>(list)).Returns(mapped);

            var result = await _service.GetAllProjectSectionAsync();

            Assert.Single(result);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetProjectSectionByIdAsync_Should_Return_Dto()
        {
            var id = "1";

            var entity = new ProjectSection { Id = id, Title = "Test" };
            var dto = new GetProjectSectionByIdDto { Title = "Test" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(entity);
            _mockMapper.Setup(x => x.Map<GetProjectSectionByIdDto>(entity)).Returns(dto);

            var result = await _service.GetProjectSectionByIdAsync(id);

            Assert.Equal("Test", result.Title);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateProjectSectionAsync_Should_Call_Repository()
        {
            var dto = new UpdateProjectSectionDto
            {
                Id = "1",
                Title = "Updated"
            };

            _mockMapper.Setup(x => x.Map<ProjectSection>(dto))
                .Returns(new ProjectSection { Id = dto.Id });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<ProjectSection>()))
                .Returns(Task.CompletedTask);

            await _service.UpdateProjectSectionAsync(dto);

            _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<ProjectSection>()), Times.Once);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteProjectSectionAsync_Should_Call_Repository()
        {
            var id = "1";

            _mockRepository.Setup(x => x.DeleteAsync(id))
                .Returns(Task.CompletedTask);

            await _service.DeleteProjectSectionAsync(id);

            _mockRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        #endregion
    }
}