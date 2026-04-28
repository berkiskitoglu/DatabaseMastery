using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.GetInTouchDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.GetInTouchServices;
using Moq;
using Xunit;

namespace TransportMongoDb.Tests.UnitTests.Services
{
    public class GetInTouchServiceTests
    {
        private readonly Mock<IGenericRepository<GetInTouch>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetInTouchService _service;

        public GetInTouchServiceTests()
        {
            _mockRepository = new Mock<IGenericRepository<GetInTouch>>();
            _mockMapper = new Mock<IMapper>();
            _service = new GetInTouchService(_mockRepository.Object, _mockMapper.Object);
        }

        #region CREATE

        [Fact]
        public async Task CreateGetInTouchAsync_Should_Call_Repository_Once()
        {
            var dto = new CreateGetInTouchDto
            {
                MainTitle = "Test",
                ImageUrl = "img.jpg"
            };

            _mockMapper.Setup(x => x.Map<GetInTouch>(dto))
                .Returns(new GetInTouch { MainTitle = dto.MainTitle });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<GetInTouch>()))
                .Returns(Task.CompletedTask);

            await _service.CreateGetInTouchAsync(dto);

            _mockRepository.Verify(x => x.CreateAsync(It.IsAny<GetInTouch>()), Times.Once);
        }

        [Fact]
        public async Task CreateGetInTouchAsync_Should_Pass_Correct_Data()
        {
            var dto = new CreateGetInTouchDto
            {
                MainTitle = "Main",
                Description = "Desc",
                ImageUrl = "img.jpg"
            };

            _mockMapper.Setup(x => x.Map<GetInTouch>(dto))
                .Returns(new GetInTouch
                {
                    MainTitle = dto.MainTitle,
                    Description = dto.Description,
                    ImageUrl = dto.ImageUrl
                });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<GetInTouch>()))
                .Returns(Task.CompletedTask);

            await _service.CreateGetInTouchAsync(dto);

            _mockRepository.Verify(x => x.CreateAsync(It.Is<GetInTouch>(x =>
                x.MainTitle == "Main" &&
                x.Description == "Desc" &&
                x.ImageUrl == "img.jpg"
            )), Times.Once);
        }

        [Fact]
        public async Task CreateGetInTouchAsync_Should_Handle_Exception()
        {
            var dto = new CreateGetInTouchDto { MainTitle = "Test" };

            _mockMapper.Setup(x => x.Map<GetInTouch>(dto))
                .Returns(new GetInTouch());

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<GetInTouch>()))
                .ThrowsAsync(new Exception("DB Error"));

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                _service.CreateGetInTouchAsync(dto));

            Assert.Equal("DB Error", ex.Message);
        }

        #endregion

        #region GET ALL

        [Fact]
        public async Task GetAllGetInTouchAsync_Should_Call_Repository()
        {
            var list = new List<GetInTouch>
            {
                new GetInTouch { MainTitle = "A" },
                new GetInTouch { MainTitle = "B" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(list);
            _mockMapper.Setup(x => x.Map<List<ResultGetInTouchDto>>(list))
                .Returns(new List<ResultGetInTouchDto>());

            await _service.GetAllGetInTouchAsync();

            _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllGetInTouchAsync_Should_Return_Mapped_Data()
        {
            var list = new List<GetInTouch>
            {
                new GetInTouch { MainTitle = "Test" }
            };

            var mapped = new List<ResultGetInTouchDto>
            {
                new ResultGetInTouchDto { MainTitle = "Test" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(list);
            _mockMapper.Setup(x => x.Map<List<ResultGetInTouchDto>>(list)).Returns(mapped);

            var result = await _service.GetAllGetInTouchAsync();

            Assert.Equal(mapped, result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllGetInTouchAsync_Should_Return_Empty()
        {
            var empty = new List<GetInTouch>();

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(empty);
            _mockMapper.Setup(x => x.Map<List<ResultGetInTouchDto>>(empty))
                .Returns(new List<ResultGetInTouchDto>());

            var result = await _service.GetAllGetInTouchAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GET BY ID

        [Fact]
        public async Task GetGetInTouchByIdAsync_Should_Call_Repository()
        {
            var id = "507f1f77bcf86cd799439011";
            var entity = new GetInTouch { Id = id };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(entity);
            _mockMapper.Setup(x => x.Map<GetInTouchByIdDto>(entity))
                .Returns(new GetInTouchByIdDto());

            await _service.GetInTouchByIdAsync(id);

            _mockRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetGetInTouchByIdAsync_Should_Return_Data()
        {
            var id = "507f1f77bcf86cd799439011";
            var entity = new GetInTouch { Id = id, MainTitle = "Test" };
            var dto = new GetInTouchByIdDto { MainTitle = "Test" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(entity);
            _mockMapper.Setup(x => x.Map<GetInTouchByIdDto>(entity)).Returns(dto);

            var result = await _service.GetInTouchByIdAsync(id);

            Assert.Equal(dto, result);
            Assert.Equal("Test", result.MainTitle);
        }

        #endregion

        #region UPDATE

        [Fact]
        public async Task UpdateGetInTouchAsync_Should_Call_Repository()
        {
            var dto = new UpdateGetInTouchDto
            {
                Id = "507f1f77bcf86cd799439011",
                MainTitle = "Updated"
            };

            _mockMapper.Setup(x => x.Map<GetInTouch>(dto))
                .Returns(new GetInTouch { Id = dto.Id });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<GetInTouch>()))
                .Returns(Task.CompletedTask);

            await _service.UpdateGetInTouchAsync(dto);

            _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<GetInTouch>()), Times.Once);
        }

        [Fact]
        public async Task UpdateGetInTouchAsync_Should_Pass_Correct_Data()
        {
            var dto = new UpdateGetInTouchDto
            {
                Id = "507f1f77bcf86cd799439011",
                MainTitle = "Updated"
            };

            _mockMapper.Setup(x => x.Map<GetInTouch>(dto))
                .Returns(new GetInTouch
                {
                    Id = dto.Id,
                    MainTitle = dto.MainTitle
                });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<GetInTouch>()))
                .Returns(Task.CompletedTask);

            await _service.UpdateGetInTouchAsync(dto);

            _mockRepository.Verify(x => x.UpdateAsync(It.Is<GetInTouch>(x =>
                x.Id == dto.Id &&
                x.MainTitle == "Updated"
            )), Times.Once);
        }

        #endregion

        #region DELETE

        [Fact]
        public async Task DeleteGetInTouchAsync_Should_Call_Repository()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockRepository.Setup(x => x.DeleteAsync(id))
                .Returns(Task.CompletedTask);

            await _service.DeleteGetInTouchAsync(id);

            _mockRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteGetInTouchAsync_Should_Pass_Correct_Id()
        {
            var id = "507f1f77bcf86cd799439011";
            string capturedId = null;

            _mockRepository.Setup(x => x.DeleteAsync(It.IsAny<string>()))
                .Callback<string>(x => capturedId = x)
                .Returns(Task.CompletedTask);

            await _service.DeleteGetInTouchAsync(id);

            Assert.Equal(id, capturedId);
        }

        #endregion
    }
}