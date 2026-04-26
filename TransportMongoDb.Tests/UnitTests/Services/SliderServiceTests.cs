using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.SliderDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransportMongoDb.Tests.UnitTests.Services
{
    public class SliderServiceTests
    {
        private readonly Mock<IGenericRepository<Slider>> _mockRepository;
        private readonly SliderService _sliderService;
        private readonly Mock<IMapper> _mockMapper;

        public SliderServiceTests()
        {
            _mockRepository = new Mock<IGenericRepository<Slider>>();
            _mockMapper = new Mock<IMapper>();
            _sliderService = new SliderService(_mockRepository.Object, _mockMapper.Object);
        }

        #region CreateSliderAsync Tests

        [Fact]
        public async Task CreateSliderAsync_Should_Call_Repository_CreateAsync_Once()
        {
            // Arrange
            var createDto = new CreateSliderDto
            {
                Title = "New Slider",
                ImageUrl = "new.jpg"
            };

            _mockMapper.Setup(x => x.Map<Slider>(createDto)).Returns(new Slider
            {
                Title = createDto.Title,
                ImageUrl = createDto.ImageUrl
            });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Slider>())).Returns(Task.CompletedTask);

            // Act
            await _sliderService.CreateSliderAsync(createDto);

            // Assert
            _mockRepository.Verify(x => x.CreateAsync(It.IsAny<Slider>()), Times.Once, "Repository CreateAsync should be called exactly once");
        }

        [Fact]
        public async Task CreateSliderAsync_Should_Pass_Correct_Data_To_Repository()
        {
            // Arrange
            var createDto = new CreateSliderDto
            {
                Title = "My Slider",
                ImageUrl = "my-slider.jpg"
            };

            _mockMapper.Setup(x => x.Map<Slider>(createDto)).Returns(new Slider
            {
                Title = createDto.Title,
                ImageUrl = createDto.ImageUrl
            });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Slider>())).Returns(Task.CompletedTask);

            // Act
            await _sliderService.CreateSliderAsync(createDto);

            // Assert
            _mockRepository.Verify(x => x.CreateAsync(It.Is<Slider>(s =>
                s.Title == "My Slider" &&
                s.ImageUrl == "my-slider.jpg"
            )), Times.Once);
        }

        [Fact]
        public async Task CreateSliderAsync_Should_Handle_Repository_Exception()
        {
            // Arrange
            var createDto = new CreateSliderDto
            {
                Title = "Test",
                ImageUrl = "test.jpg"
            };

            _mockMapper
                .Setup(x => x.Map<Slider>(It.IsAny<CreateSliderDto>()))
                .Returns(new Slider { Title = createDto.Title, ImageUrl = createDto.ImageUrl });

            _mockRepository
                .Setup(x => x.CreateAsync(It.IsAny<Slider>()))
                .ThrowsAsync(new Exception("Database Connection Failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _sliderService.CreateSliderAsync(createDto));

            Assert.Equal("Database Connection Failed", exception.Message);
        }

        #endregion

        #region GetAllSliderAsync Tests

        [Fact]
        public async Task GetAllSliderAsync_Should_Call_Repository_GetAllAsync_Once()
        {
            // Arrange
            var sliders = new List<Slider>
            {
                new Slider { Title = "Slider 1", ImageUrl = "img1.jpg" },
                new Slider { Title = "Slider 2", ImageUrl = "img2.jpg" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(sliders);
            _mockMapper.Setup(x => x.Map<List<ResultSliderDto>>(sliders)).Returns(new List<ResultSliderDto>());

            // Act
            await _sliderService.GetAllSliderAsync();

            // Assert
            _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllSliderAsync_Should_Return_Mapped_Dtos()
        {
            // Arrange
            var sliders = new List<Slider>
            {
                new Slider { Title = "Slider 1", ImageUrl = "img1.jpg" }
            };

            var expectedDtos = new List<ResultSliderDto>
            {
                new ResultSliderDto { Title = "Slider 1" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(sliders);
            _mockMapper.Setup(x => x.Map<List<ResultSliderDto>>(sliders)).Returns(expectedDtos);

            // Act
            var result = await _sliderService.GetAllSliderAsync();

            // Assert
            Assert.Equal(expectedDtos, result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllSliderAsync_Should_Return_Empty_List_When_No_Sliders()
        {
            // Arrange
            var emptyList = new List<Slider>();

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(emptyList);
            _mockMapper.Setup(x => x.Map<List<ResultSliderDto>>(emptyList)).Returns(new List<ResultSliderDto>());

            // Act
            var result = await _sliderService.GetAllSliderAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetSliderByIdAsync Tests

        [Fact]
        public async Task GetSliderByIdAsync_Should_Call_Repository_GetByIdAsync_Once()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            var slider = new Slider { Id = id, Title = "Test Slider" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(slider);
            _mockMapper.Setup(x => x.Map<GetSliderByIdDto>(slider)).Returns(new GetSliderByIdDto());

            // Act
            await _sliderService.GetSliderByIdAsync(id);

            // Assert
            _mockRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetSliderByIdAsync_Should_Return_Mapped_Dto()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            var slider = new Slider { Id = id, Title = "Test Slider" };
            var expectedDto = new GetSliderByIdDto { Title = "Test Slider" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(slider);
            _mockMapper.Setup(x => x.Map<GetSliderByIdDto>(slider)).Returns(expectedDto);

            // Act
            var result = await _sliderService.GetSliderByIdAsync(id);

            // Assert
            Assert.Equal(expectedDto, result);
            Assert.Equal("Test Slider", result.Title);
        }

        #endregion

        #region UpdateSliderAsync Tests

        [Fact]
        public async Task UpdateSliderAsync_Should_Call_Repository_UpdateAsync_Once()
        {
            // Arrange
            var updateDto = new UpdateSliderDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated Slider",
                ImageUrl = "updated.jpg"
            };

            _mockMapper.Setup(x => x.Map<Slider>(updateDto)).Returns(new Slider
            {
                Id = updateDto.Id,
                Title = updateDto.Title,
                ImageUrl = updateDto.ImageUrl
            });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Slider>())).Returns(Task.CompletedTask);

            // Act
            await _sliderService.UpdateSliderAsync(updateDto);

            // Assert
            _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Slider>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSliderAsync_Should_Pass_Correct_Data_To_Repository()
        {
            // Arrange
            var updateDto = new UpdateSliderDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated Slider",
                ImageUrl = "updated.jpg"
            };

            _mockMapper.Setup(x => x.Map<Slider>(updateDto)).Returns(new Slider
            {
                Id = updateDto.Id,
                Title = updateDto.Title,
                ImageUrl = updateDto.ImageUrl
            });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Slider>())).Returns(Task.CompletedTask);

            // Act
            await _sliderService.UpdateSliderAsync(updateDto);

            // Assert
            _mockRepository.Verify(x => x.UpdateAsync(It.Is<Slider>(s =>
                s.Id == "507f1f77bcf86cd799439011" &&
                s.Title == "Updated Slider" &&
                s.ImageUrl == "updated.jpg"
            )), Times.Once);
        }

        #endregion

        #region DeleteSliderAsync Tests

        [Fact]
        public async Task DeleteSliderAsync_Should_Call_Repository_DeleteAsync_Once()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";

            _mockRepository.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            await _sliderService.DeleteSliderAsync(id);

            // Assert
            _mockRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteSliderAsync_Should_Pass_Correct_Id_To_Repository()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            string capturedId = null;

            _mockRepository
                .Setup(x => x.DeleteAsync(It.IsAny<string>()))
                .Callback<string>(receivedId => capturedId = receivedId)
                .Returns(Task.CompletedTask);

            // Act
            await _sliderService.DeleteSliderAsync(id);

            // Assert
            Assert.Equal(id, capturedId);
        }

        #endregion
    }
}