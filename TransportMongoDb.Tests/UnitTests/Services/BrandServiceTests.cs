using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.BrandDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransportMongoDb.Tests.UnitTests.Services
{
    public class BrandServiceTests
    {
        private readonly Mock<IGenericRepository<Brand>> _mockRepository;
        private readonly BrandService _brandService;
        private readonly Mock<IMapper> _mockMapper;

        public BrandServiceTests()
        {
            _mockRepository = new Mock<IGenericRepository<Brand>>();
            _mockMapper = new Mock<IMapper>();
            _brandService = new BrandService(_mockRepository.Object, _mockMapper.Object);
        }

        #region CreateBrandAsync Tests

        [Fact]
        public async Task CreateBrandAsync_Should_Call_Repository_CreateAsync_Once()
        {
            // Arrange
            var createDto = new CreateBrandDto
            {
                BrandName = "New Brand",
                ImageUrl = "new.jpg"
            };

            _mockMapper.Setup(x => x.Map<Brand>(createDto)).Returns(new Brand
            {
                BrandName = createDto.BrandName,
                ImageUrl = createDto.ImageUrl
            });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Brand>())).Returns(Task.CompletedTask);

            // Act
            await _brandService.CreateBrandAsync(createDto);

            // Assert
            _mockRepository.Verify(x => x.CreateAsync(It.IsAny<Brand>()), Times.Once, "Repository CreateAsync should be called exactly once");
        }

        [Fact]
        public async Task CreateBrandAsync_Should_Pass_Correct_Data_To_Repository()
        {
            // Arrange
            var createDto = new CreateBrandDto
            {
                BrandName = "My Brand",
                ImageUrl = "my-brand.jpg"
            };

            _mockMapper.Setup(x => x.Map<Brand>(createDto)).Returns(new Brand
            {
                BrandName = createDto.BrandName,
                ImageUrl = createDto.ImageUrl
            });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Brand>())).Returns(Task.CompletedTask);

            // Act
            await _brandService.CreateBrandAsync(createDto);

            // Assert
            _mockRepository.Verify(x => x.CreateAsync(It.Is<Brand>(b =>
                b.BrandName == "My Brand" &&
                b.ImageUrl == "my-brand.jpg"
            )), Times.Once);
        }

        [Fact]
        public async Task CreateBrandAsync_Should_Handle_Repository_Exception()
        {
            // Arrange
            var createDto = new CreateBrandDto
            {
                BrandName = "Test Brand",
                ImageUrl = "test.jpg"
            };

            _mockMapper
                .Setup(x => x.Map<Brand>(It.IsAny<CreateBrandDto>()))
                .Returns(new Brand { BrandName = createDto.BrandName, ImageUrl = createDto.ImageUrl });

            _mockRepository
                .Setup(x => x.CreateAsync(It.IsAny<Brand>()))
                .ThrowsAsync(new Exception("Database Connection Failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _brandService.CreateBrandAsync(createDto));

            Assert.Equal("Database Connection Failed", exception.Message);
        }

        #endregion

        #region GetAllBrandAsync Tests

        [Fact]
        public async Task GetAllBrandAsync_Should_Call_Repository_GetAllAsync_Once()
        {
            // Arrange
            var brands = new List<Brand>
            {
                new Brand { BrandName = "Brand 1", ImageUrl = "img1.jpg" },
                new Brand { BrandName = "Brand 2", ImageUrl = "img2.jpg" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(brands);
            _mockMapper.Setup(x => x.Map<List<ResultBrandDto>>(brands)).Returns(new List<ResultBrandDto>());

            // Act
            await _brandService.GetAllBrandAsync();

            // Assert
            _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllBrandAsync_Should_Return_Mapped_Dtos()
        {
            // Arrange
            var brands = new List<Brand>
            {
                new Brand { BrandName = "Brand 1", ImageUrl = "img1.jpg" }
            };

            var expectedDtos = new List<ResultBrandDto>
            {
                new ResultBrandDto { BrandName = "Brand 1" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(brands);
            _mockMapper.Setup(x => x.Map<List<ResultBrandDto>>(brands)).Returns(expectedDtos);

            // Act
            var result = await _brandService.GetAllBrandAsync();

            // Assert
            Assert.Equal(expectedDtos, result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllBrandAsync_Should_Return_Empty_List_When_No_Brands()
        {
            // Arrange
            var emptyList = new List<Brand>();

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(emptyList);
            _mockMapper.Setup(x => x.Map<List<ResultBrandDto>>(emptyList)).Returns(new List<ResultBrandDto>());

            // Act
            var result = await _brandService.GetAllBrandAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetBrandByIdAsync Tests

        [Fact]
        public async Task GetBrandByIdAsync_Should_Call_Repository_GetByIdAsync_Once()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            var brand = new Brand { Id = id, BrandName = "Test Brand" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(brand);
            _mockMapper.Setup(x => x.Map<GetBrandByIdDto>(brand)).Returns(new GetBrandByIdDto());

            // Act
            await _brandService.GetBrandByIdAsync(id);

            // Assert
            _mockRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetBrandByIdAsync_Should_Return_Mapped_Dto()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            var brand = new Brand { Id = id, BrandName = "Test Brand" };
            var expectedDto = new GetBrandByIdDto { BrandName = "Test Brand" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(brand);
            _mockMapper.Setup(x => x.Map<GetBrandByIdDto>(brand)).Returns(expectedDto);

            // Act
            var result = await _brandService.GetBrandByIdAsync(id);

            // Assert
            Assert.Equal(expectedDto, result);
            Assert.Equal("Test Brand", result.BrandName);
        }

        #endregion

        #region UpdateBrandAsync Tests

        [Fact]
        public async Task UpdateBrandAsync_Should_Call_Repository_UpdateAsync_Once()
        {
            // Arrange
            var updateDto = new UpdateBrandDto
            {
                Id = "507f1f77bcf86cd799439011",
                BrandName = "Updated Brand",
                ImageUrl = "updated.jpg"
            };

            _mockMapper.Setup(x => x.Map<Brand>(updateDto)).Returns(new Brand
            {
                Id = updateDto.Id,
                BrandName = updateDto.BrandName,
                ImageUrl = updateDto.ImageUrl
            });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Brand>())).Returns(Task.CompletedTask);

            // Act
            await _brandService.UpdateBrandAsync(updateDto);

            // Assert
            _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Brand>()), Times.Once);
        }

        [Fact]
        public async Task UpdateBrandAsync_Should_Pass_Correct_Data_To_Repository()
        {
            // Arrange
            var updateDto = new UpdateBrandDto
            {
                Id = "507f1f77bcf86cd799439011",
                BrandName = "Updated Brand",
                ImageUrl = "updated.jpg"
            };

            _mockMapper.Setup(x => x.Map<Brand>(updateDto)).Returns(new Brand
            {
                Id = updateDto.Id,
                BrandName = updateDto.BrandName,
                ImageUrl = updateDto.ImageUrl
            });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Brand>())).Returns(Task.CompletedTask);

            // Act
            await _brandService.UpdateBrandAsync(updateDto);

            // Assert
            _mockRepository.Verify(x => x.UpdateAsync(It.Is<Brand>(b =>
                b.Id == "507f1f77bcf86cd799439011" &&
                b.BrandName == "Updated Brand" &&
                b.ImageUrl == "updated.jpg"
            )), Times.Once);
        }

        #endregion

        #region DeleteBrandAsync Tests

        [Fact]
        public async Task DeleteBrandAsync_Should_Call_Repository_DeleteAsync_Once()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";

            _mockRepository.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            await _brandService.DeleteBrandAsync(id);

            // Assert
            _mockRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteBrandAsync_Should_Pass_Correct_Id_To_Repository()
        {
            // Arrange
            var id = "507f1f77bcf86cd799439011";
            string capturedId = null;

            _mockRepository
                .Setup(x => x.DeleteAsync(It.IsAny<string>()))
                .Callback<string>(receivedId => capturedId = receivedId)
                .Returns(Task.CompletedTask);

            // Act
            await _brandService.DeleteBrandAsync(id);

            // Assert
            Assert.Equal(id, capturedId);
        }

        #endregion
    }
}