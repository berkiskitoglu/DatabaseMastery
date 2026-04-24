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
        // Unit Test With Mock
        private readonly Mock<IGenericRepository<Slider>> _mockRepository;
        private readonly SliderService _sliderService;
        private readonly Mock<IMapper> _mockMapper;

        public SliderServiceTests()
        {
            _mockRepository = new Mock<IGenericRepository<Slider>>();
            _mockMapper = new Mock<IMapper>();
            _sliderService = new SliderService(_mockRepository.Object, _mockMapper.Object);
        }

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

            //Assert  - Doğru Veri Gönderildimi
            _mockRepository.Verify(x => x.CreateAsync(It.Is<Slider>(s => s.Title == "My Slider" && s.ImageUrl == "my-slider.jpg")), Times.Once);
        }


        #region
        // Fluent Validation veya validation kullanıldığı zaman aşşağıdaki testi gerçekleştirebiliriz.

        //[Fact]
        //public async Task CreateSliderAsync_Should_Throw_When_Title_Is_Empty()
        //{
        //    // Arrange
        //    var createDto = new CreateSliderDto
        //    {
        //        Title = "",
        //        ImageUrl = "test.jpg"
        //    };

        //    // Act & Assert
        //    await Assert.ThrowsAsync<ArgumentException>(() => _sliderService.CreateSliderAsync(createDto));

        //    // Repository Çağrılmamış Olacak
        //    _mockRepository.Verify(x => x.CreateAsync(It.IsAny<Slider>()), Times.Never);
        //
        #endregion


        // Repository hatayı Yakalayıp Exception Fırlatıyor mu

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
                .Returns(new Slider
                {
                    Title = createDto.Title,
                    ImageUrl = createDto.ImageUrl
                });

            _mockRepository
                .Setup(x => x.CreateAsync(It.IsAny<Slider>()))
                .ThrowsAsync(new Exception("Database Connection Failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _sliderService.CreateSliderAsync(createDto));

            Assert.Equal("Database Connection Failed", exception.Message);
        }
    }
}


