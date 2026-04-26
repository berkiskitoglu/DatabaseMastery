using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.OfferDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransportMongoDb.Tests.UnitTests.Services
{
    public class OfferServiceTests
    {
        private readonly Mock<IGenericRepository<Offer>> _mockRepository;
        private readonly OfferService _offerService;
        private readonly Mock<IMapper> _mockMapper;

        public OfferServiceTests()
        {
            _mockRepository = new Mock<IGenericRepository<Offer>>();
            _mockMapper = new Mock<IMapper>();
            _offerService = new OfferService(_mockRepository.Object, _mockMapper.Object);
        }

        #region CreateOfferAsync Tests

        [Fact]
        public async Task CreateOfferAsync_Should_Call_Repository_CreateAsync_Once()
        {
            // Arrange
            var createDto = new CreateOfferDto
            {
                Title = "New Offer",
                ImageUrl = "new.jpg"
            };

            _mockMapper.Setup(x => x.Map<Offer>(createDto)).Returns(new Offer
            {
                Title = createDto.Title,
                ImageUrl = createDto.ImageUrl
            });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Offer>())).Returns(Task.CompletedTask);

            // Act
            await _offerService.CreateOfferAsync(createDto);

            // Assert
            _mockRepository.Verify(x => x.CreateAsync(It.IsAny<Offer>()), Times.Once, "Repository CreateAsync should be called exactly once");
        }

        [Fact]
        public async Task CreateOfferAsync_Should_Pass_Correct_Data_To_Repository()
        {
            // Arrange
            var createDto = new CreateOfferDto
            {
                Title = "My Offer",
                ImageUrl = "my-offer.jpg"
            };

            _mockMapper.Setup(x => x.Map<Offer>(createDto)).Returns(new Offer
            {
                Title = createDto.Title,
                ImageUrl = createDto.ImageUrl
            });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Offer>())).Returns(Task.CompletedTask);

            // Act
            await _offerService.CreateOfferAsync(createDto);

            // Assert
            _mockRepository.Verify(x => x.CreateAsync(It.Is<Offer>(o =>
                o.Title == "My Offer" &&
                o.ImageUrl == "my-offer.jpg"
            )), Times.Once);
        }

        [Fact]
        public async Task CreateOfferAsync_Should_Handle_Repository_Exception()
        {
            // Arrange
            var createDto = new CreateOfferDto
            {
                Title = "Test Offer",
                ImageUrl = "test.jpg"
            };

            _mockMapper
                .Setup(x => x.Map<Offer>(It.IsAny<CreateOfferDto>()))
                .Returns(new Offer { Title = createDto.Title, ImageUrl = createDto.ImageUrl });

            _mockRepository
                .Setup(x => x.CreateAsync(It.IsAny<Offer>()))
                .ThrowsAsync(new Exception("Database Connection Failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _offerService.CreateOfferAsync(createDto));

            Assert.Equal("Database Connection Failed", exception.Message);
        }

        #endregion

        #region GetAllOfferAsync Tests

        [Fact]
        public async Task GetAllOfferAsync_Should_Call_Repository_GetAllAsync_Once()
        {
            // Arrange
            var offers = new List<Offer>
            {
                new Offer { Title = "Offer 1", ImageUrl = "img1.jpg" },
                new Offer { Title = "Offer 2", ImageUrl = "img2.jpg" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(offers);
            _mockMapper.Setup(x => x.Map<List<ResultOfferDto>>(offers)).Returns(new List<ResultOfferDto>());

            // Act
            await _offerService.GetAllOfferAsync();

            // Assert
            _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllOfferAsync_Should_Return_Mapped_Dtos()
        {
            // Arrange
            var offers = new List<Offer>
            {
                new Offer { Title = "Offer 1", ImageUrl = "img1.jpg" }
            };

            var expectedDtos = new List<ResultOfferDto>
            {
                new ResultOfferDto { Title = "Offer 1" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(offers);
            _mockMapper.Setup(x => x.Map<List<ResultOfferDto>>(offers)).Returns(expectedDtos);

            // Act
            var result = await _offerService.GetAllOfferAsync();

            // Assert
            Assert.Equal(expectedDtos, result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllOfferAsync_Should_Return_Empty_List_When_No_Offers()
        {
            // Arrange
            var emptyList = new List<Offer>();

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(emptyList);
            _mockMapper.Setup(x => x.Map<List<ResultOfferDto>>(emptyList)).Returns(new List<ResultOfferDto>());

            // Act
            var result = await _offerService.GetAllOfferAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetOfferByIdAsync Tests

        [Fact]
        public async Task GetOfferByIdAsync_Should_Call_Repository_GetByIdAsync_Once()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            var offer = new Offer { Id = id, Title = "Test Offer" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(offer);
            _mockMapper.Setup(x => x.Map<GetOfferByIdDto>(offer)).Returns(new GetOfferByIdDto());

            // Act
            await _offerService.GetOfferByIdAsync(id);

            // Assert
            _mockRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetOfferByIdAsync_Should_Return_Mapped_Dto()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            var offer = new Offer { Id = id, Title = "Test Offer" };
            var expectedDto = new GetOfferByIdDto { Title = "Test Offer" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(offer);
            _mockMapper.Setup(x => x.Map<GetOfferByIdDto>(offer)).Returns(expectedDto);

            // Act
            var result = await _offerService.GetOfferByIdAsync(id);

            // Assert
            Assert.Equal(expectedDto, result);
            Assert.Equal("Test Offer", result.Title);
        }

        #endregion

        #region UpdateOfferAsync Tests

        [Fact]
        public async Task UpdateOfferAsync_Should_Call_Repository_UpdateAsync_Once()
        {
            // Arrange
            var updateDto = new UpdateOfferDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated Offer",
                ImageUrl = "updated.jpg"
            };

            _mockMapper.Setup(x => x.Map<Offer>(updateDto)).Returns(new Offer
            {
                Id = updateDto.Id,
                Title = updateDto.Title,
                ImageUrl = updateDto.ImageUrl
            });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Offer>())).Returns(Task.CompletedTask);

            // Act
            await _offerService.UpdateOfferAsync(updateDto);

            // Assert
            _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Offer>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOfferAsync_Should_Pass_Correct_Data_To_Repository()
        {
            // Arrange
            var updateDto = new UpdateOfferDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated Offer",
                ImageUrl = "updated.jpg"
            };

            _mockMapper.Setup(x => x.Map<Offer>(updateDto)).Returns(new Offer
            {
                Id = updateDto.Id,
                Title = updateDto.Title,
                ImageUrl = updateDto.ImageUrl
            });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Offer>())).Returns(Task.CompletedTask);

            // Act
            await _offerService.UpdateOfferAsync(updateDto);

            // Assert
            _mockRepository.Verify(x => x.UpdateAsync(It.Is<Offer>(o =>
                o.Id == "507f1f77bcf86cd799439011" &&
                o.Title == "Updated Offer" &&
                o.ImageUrl == "updated.jpg"
            )), Times.Once);
        }

        #endregion

        #region DeleteOfferAsync Tests

        [Fact]
        public async Task DeleteOfferAsync_Should_Call_Repository_DeleteAsync_Once()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";

            _mockRepository.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            await _offerService.DeleteOfferAsync(id);

            // Assert
            _mockRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteOfferAsync_Should_Pass_Correct_Id_To_Repository()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            string capturedId = null;

            _mockRepository
                .Setup(x => x.DeleteAsync(It.IsAny<string>()))
                .Callback<string>(receivedId => capturedId = receivedId)
                .Returns(Task.CompletedTask);

            // Act
            await _offerService.DeleteOfferAsync(id);

            // Assert
            Assert.Equal(id, capturedId);
        }

        #endregion
    }
}