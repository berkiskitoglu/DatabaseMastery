using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.AboutDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.AboutServices;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TransportMongoDb.Tests.UnitTests.Services
{
    public class AboutServiceTests
    {
        private readonly Mock<IGenericRepository<About>> _mockRepository;
        private readonly AboutService _aboutService;
        private readonly Mock<IMapper> _mockMapper;

        public AboutServiceTests()
        {
            _mockRepository = new Mock<IGenericRepository<About>>();
            _mockMapper = new Mock<IMapper>();
            _aboutService = new AboutService(_mockRepository.Object, _mockMapper.Object);
        }

        #region CreateAboutAsync Tests

        [Fact]
        public async Task CreateAboutAsync_Should_Call_Repository_CreateAsync_Once()
        {
            var createDto = new CreateAboutDto
            {
                Title = "New About",
                Description = "Desc",
                ImageUrl = "new.jpg"
            };

            _mockMapper.Setup(x => x.Map<About>(createDto)).Returns(new About
            {
                Title = createDto.Title,
                Description = createDto.Description,
                ImageUrl = createDto.ImageUrl
            });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<About>()))
                           .Returns(Task.CompletedTask);

            await _aboutService.CreateAboutAsync(createDto);

            _mockRepository.Verify(x => x.CreateAsync(It.IsAny<About>()), Times.Once);
        }

        [Fact]
        public async Task CreateAboutAsync_Should_Pass_Correct_Data_To_Repository()
        {
            var createDto = new CreateAboutDto
            {
                Title = "My About",
                Description = "My Desc",
                ImageUrl = "my.jpg"
            };

            _mockMapper.Setup(x => x.Map<About>(createDto)).Returns(new About
            {
                Title = createDto.Title,
                Description = createDto.Description,
                ImageUrl = createDto.ImageUrl
            });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<About>()))
                           .Returns(Task.CompletedTask);

            await _aboutService.CreateAboutAsync(createDto);

            _mockRepository.Verify(x => x.CreateAsync(It.Is<About>(a =>
                a.Title == "My About" &&
                a.Description == "My Desc" &&
                a.ImageUrl == "my.jpg"
            )), Times.Once);
        }

        [Fact]
        public async Task CreateAboutAsync_Should_Handle_Repository_Exception()
        {
            var createDto = new CreateAboutDto
            {
                Title = "Test",
                Description = "Desc",
                ImageUrl = "test.jpg"
            };

            _mockMapper.Setup(x => x.Map<About>(It.IsAny<CreateAboutDto>()))
                       .Returns(new About());

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<About>()))
                           .ThrowsAsync(new Exception("Database Error"));

            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _aboutService.CreateAboutAsync(createDto));

            Assert.Equal("Database Error", exception.Message);
        }

        #endregion

        #region GetAllAboutAsync Tests

        [Fact]
        public async Task GetAllAboutAsync_Should_Call_Repository_GetAllAsync_Once()
        {
            var abouts = new List<About>
            {
                new About { Title = "About 1" },
                new About { Title = "About 2" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(abouts);
            _mockMapper.Setup(x => x.Map<List<ResultAboutDto>>(abouts))
                       .Returns(new List<ResultAboutDto>());

            await _aboutService.GetAllAboutAsync();

            _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAboutAsync_Should_Return_Mapped_Dtos()
        {
            var abouts = new List<About>
            {
                new About { Title = "About 1" }
            };

            var expectedDtos = new List<ResultAboutDto>
            {
                new ResultAboutDto { Title = "About 1" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(abouts);
            _mockMapper.Setup(x => x.Map<List<ResultAboutDto>>(abouts))
                       .Returns(expectedDtos);

            var result = await _aboutService.GetAllAboutAsync();

            Assert.Equal(expectedDtos, result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllAboutAsync_Should_Return_Empty_List()
        {
            var emptyList = new List<About>();

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(emptyList);
            _mockMapper.Setup(x => x.Map<List<ResultAboutDto>>(emptyList))
                       .Returns(new List<ResultAboutDto>());

            var result = await _aboutService.GetAllAboutAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetAboutByIdAsync Tests

        [Fact]
        public async Task GetAboutByIdAsync_Should_Call_Repository_GetByIdAsync_Once()
        {
            var id = "507f1f77bcf86cd799439011";
            var about = new About { Id = id, Title = "Test" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(about);
            _mockMapper.Setup(x => x.Map<GetAboutByIdDto>(about))
                       .Returns(new GetAboutByIdDto());

            await _aboutService.GetAboutByIdAsync(id);

            _mockRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetAboutByIdAsync_Should_Return_Mapped_Dto()
        {
            var id = "507f1f77bcf86cd799439011";
            var about = new About { Id = id, Title = "Test" };
            var expectedDto = new GetAboutByIdDto { Title = "Test" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(about);
            _mockMapper.Setup(x => x.Map<GetAboutByIdDto>(about))
                       .Returns(expectedDto);

            var result = await _aboutService.GetAboutByIdAsync(id);

            Assert.Equal(expectedDto, result);
            Assert.Equal("Test", result.Title);
        }

        #endregion

        #region UpdateAboutAsync Tests

        [Fact]
        public async Task UpdateAboutAsync_Should_Call_Repository_UpdateAsync_Once()
        {
            var updateDto = new UpdateAboutDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated",
                Description = "Updated Desc",
                ImageUrl = "updated.jpg"
            };

            _mockMapper.Setup(x => x.Map<About>(updateDto)).Returns(new About
            {
                Id = updateDto.Id,
                Title = updateDto.Title,
                Description = updateDto.Description,
                ImageUrl = updateDto.ImageUrl
            });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<About>()))
                           .Returns(Task.CompletedTask);

            await _aboutService.UpdateAboutAsync(updateDto);

            _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<About>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAboutAsync_Should_Pass_Correct_Data()
        {
            var updateDto = new UpdateAboutDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated",
                Description = "Updated Desc",
                ImageUrl = "updated.jpg"
            };

            _mockMapper.Setup(x => x.Map<About>(updateDto)).Returns(new About
            {
                Id = updateDto.Id,
                Title = updateDto.Title,
                Description = updateDto.Description,
                ImageUrl = updateDto.ImageUrl
            });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<About>()))
                           .Returns(Task.CompletedTask);

            await _aboutService.UpdateAboutAsync(updateDto);

            _mockRepository.Verify(x => x.UpdateAsync(It.Is<About>(a =>
                a.Id == updateDto.Id &&
                a.Title == updateDto.Title &&
                a.Description == updateDto.Description &&
                a.ImageUrl == updateDto.ImageUrl
            )), Times.Once);
        }

        #endregion

        #region DeleteAboutAsync Tests

        [Fact]
        public async Task DeleteAboutAsync_Should_Call_Repository_DeleteAsync_Once()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockRepository.Setup(x => x.DeleteAsync(id))
                           .Returns(Task.CompletedTask);

            await _aboutService.DeleteAboutAsync(id);

            _mockRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAboutAsync_Should_Pass_Correct_Id()
        {
            var id = "507f1f77bcf86cd799439011";
            string capturedId = null;

            _mockRepository.Setup(x => x.DeleteAsync(It.IsAny<string>()))
                .Callback<string>(x => capturedId = x)
                .Returns(Task.CompletedTask);

            await _aboutService.DeleteAboutAsync(id);

            Assert.Equal(id, capturedId);
        }

        #endregion
    }
}