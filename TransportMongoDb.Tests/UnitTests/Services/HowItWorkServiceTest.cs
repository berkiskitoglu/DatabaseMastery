using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.HowItWorkDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.HowItWorkServices;
using Moq;
using Xunit;

namespace TransportMongoDb.Tests.UnitTests.Services
{
    public class HowItWorkServiceTests
    {
        private readonly Mock<IGenericRepository<HowItWork>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly HowItWorkService _service;

        public HowItWorkServiceTests()
        {
            _mockRepository = new Mock<IGenericRepository<HowItWork>>();
            _mockMapper = new Mock<IMapper>();
            _service = new HowItWorkService(_mockRepository.Object, _mockMapper.Object);
        }

        #region CREATE

        [Fact]
        public async Task CreateAsync_Should_Call_Repository()
        {
            var dto = new CreateHowItWorkDto
            {
                Title = "Step",
                Description = "Desc",
                IconUrl = "icon.png",
                Status = true
            };

            _mockMapper.Setup(x => x.Map<HowItWork>(dto))
                .Returns(new HowItWork
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    IconUrl = dto.IconUrl,
                    Status = dto.Status
                });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<HowItWork>()))
                .Returns(Task.CompletedTask);

            await _service.CreateHowItWorkAsync(dto);

            _mockRepository.Verify(x => x.CreateAsync(It.IsAny<HowItWork>()), Times.Once);
        }

        #endregion

        #region GET ALL

        [Fact]
        public async Task GetAllAsync_Should_Call_Repository()
        {
            var list = new List<HowItWork>
            {
                new HowItWork { Title = "Step 1" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(list);
            _mockMapper.Setup(x => x.Map<List<ResultHowItWorkDto>>(list))
                .Returns(new List<ResultHowItWorkDto>());

            await _service.GetAllHowItWorkAsync();

            _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Mapped_Data()
        {
            var list = new List<HowItWork>
            {
                new HowItWork { Title = "Step 1" }
            };

            var mapped = new List<ResultHowItWorkDto>
            {
                new ResultHowItWorkDto { Title = "Step 1" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(list);
            _mockMapper.Setup(x => x.Map<List<ResultHowItWorkDto>>(list)).Returns(mapped);

            var result = await _service.GetAllHowItWorkAsync();

            Assert.Equal(mapped, result);
            Assert.Single(result);
        }

        #endregion

        #region GET BY ID

        [Fact]
        public async Task GetByIdAsync_Should_Call_Repository()
        {
            var id = "507f1f77bcf86cd799439011";
            var entity = new HowItWork { Id = id };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(entity);
            _mockMapper.Setup(x => x.Map<GetHowItWorkByIdDto>(entity))
                .Returns(new GetHowItWorkByIdDto());

            await _service.GetHowItWorkByIdAsync(id);

            _mockRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Mapped_Data()
        {
            var id = "507f1f77bcf86cd799439011";

            var entity = new HowItWork { Id = id, Title = "Step" };
            var dto = new GetHowItWorkByIdDto { Title = "Step" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(entity);
            _mockMapper.Setup(x => x.Map<GetHowItWorkByIdDto>(entity)).Returns(dto);

            var result = await _service.GetHowItWorkByIdAsync(id);

            Assert.Equal(dto, result);
            Assert.Equal("Step", result.Title);
        }

        #endregion

        #region UPDATE

        [Fact]
        public async Task UpdateAsync_Should_Call_Repository()
        {
            var dto = new UpdateHowItWorkDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated"
            };

            _mockMapper.Setup(x => x.Map<HowItWork>(dto))
                .Returns(new HowItWork
                {
                    Id = dto.Id,
                    Title = dto.Title
                });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<HowItWork>()))
                .Returns(Task.CompletedTask);

            await _service.UpdateHowItWorkAsync(dto);

            _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<HowItWork>()), Times.Once);
        }

        #endregion

        #region DELETE

        [Fact]
        public async Task DeleteAsync_Should_Call_Repository()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockRepository.Setup(x => x.DeleteAsync(id))
                .Returns(Task.CompletedTask);

            await _service.DeleteHowItWorkAsync(id);

            _mockRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_Pass_Correct_Id()
        {
            var id = "507f1f77bcf86cd799439011";
            string capturedId = null;

            _mockRepository.Setup(x => x.DeleteAsync(It.IsAny<string>()))
                .Callback<string>(x => capturedId = x)
                .Returns(Task.CompletedTask);

            await _service.DeleteHowItWorkAsync(id);

            Assert.Equal(id, capturedId);
        }

        #endregion
    }
}