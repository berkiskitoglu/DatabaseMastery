using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.TestimonialDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.TestimonialServices;
using Moq;

namespace TransportMongoDb.Tests.UnitTests.Services
{
    public class TestimonialServiceTests
    {
        private readonly Mock<IGenericRepository<Testimonial>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TestimonialService _service;

        public TestimonialServiceTests()
        {
            _mockRepository = new Mock<IGenericRepository<Testimonial>>();
            _mockMapper = new Mock<IMapper>();
            _service = new TestimonialService(_mockRepository.Object, _mockMapper.Object);
        }

        #region CREATE

        [Fact]
        public async Task CreateTestimonialAsync_Should_Call_Repository()
        {
            var dto = new CreateTestimonialDto
            {
                NameSurname = "John",
                ImageUrl = "img.jpg"
            };

            _mockMapper.Setup(x => x.Map<Testimonial>(dto))
                .Returns(new Testimonial { NameSurname = dto.NameSurname });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Testimonial>()))
                .Returns(Task.CompletedTask);

            await _service.CreateTestimonialAsync(dto);

            _mockRepository.Verify(x => x.CreateAsync(It.IsAny<Testimonial>()), Times.Once);
        }

        [Fact]
        public async Task CreateTestimonialAsync_Should_Send_Correct_Data()
        {
            var dto = new CreateTestimonialDto
            {
                NameSurname = "Jane",
                Title = "CEO",
                ReviewDetail = "Great",
                ImageUrl = "img.jpg",
                ReviewScore = 5,
                Status = true
            };

            _mockMapper.Setup(x => x.Map<Testimonial>(dto))
                .Returns(new Testimonial
                {
                    NameSurname = dto.NameSurname,
                    Title = dto.Title,
                    ReviewDetail = dto.ReviewDetail,
                    ImageUrl = dto.ImageUrl,
                    ReviewScore = dto.ReviewScore,
                    Status = dto.Status
                });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Testimonial>()))
                .Returns(Task.CompletedTask);

            await _service.CreateTestimonialAsync(dto);

            _mockRepository.Verify(x => x.CreateAsync(It.Is<Testimonial>(x =>
                x.NameSurname == "Jane" &&
                x.Title == "CEO" &&
                x.ReviewDetail == "Great" &&
                x.ImageUrl == "img.jpg" &&
                x.ReviewScore == 5 &&
                x.Status == true
            )), Times.Once);
        }

        #endregion

        #region GET ALL

        [Fact]
        public async Task GetAllTestimonialAsync_Should_Call_Repository()
        {
            var list = new List<Testimonial>
            {
                new Testimonial { NameSurname = "A" },
                new Testimonial { NameSurname = "B" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(list);
            _mockMapper.Setup(x => x.Map<List<ResultTestimonialDto>>(list))
                       .Returns(new List<ResultTestimonialDto>());

            await _service.GetAllTestimonialAsync();

            _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllTestimonialAsync_Should_Return_Mapped()
        {
            var list = new List<Testimonial>
            {
                new Testimonial { NameSurname = "A" }
            };

            var mapped = new List<ResultTestimonialDto>
            {
                new ResultTestimonialDto { NameSurname = "A" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(list);
            _mockMapper.Setup(x => x.Map<List<ResultTestimonialDto>>(list))
                       .Returns(mapped);

            var result = await _service.GetAllTestimonialAsync();

            Assert.Equal(mapped, result);
            Assert.Single(result);
        }

        #endregion

        #region GET BY ID

        [Fact]
        public async Task GetTestimonialByIdAsync_Should_Call_Repository()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockRepository.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(new Testimonial());

            _mockMapper.Setup(x => x.Map<GetTestimonialByIdDto>(It.IsAny<Testimonial>()))
                .Returns(new GetTestimonialByIdDto());

            await _service.GetTestimonialByIdAsync(id);

            _mockRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetTestimonialByIdAsync_Should_Return_Data()
        {
            var id = "507f1f77bcf86cd799439011";

            var entity = new Testimonial { NameSurname = "Test" };
            var dto = new GetTestimonialByIdDto { NameSurname = "Test" };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(entity);
            _mockMapper.Setup(x => x.Map<GetTestimonialByIdDto>(entity)).Returns(dto);

            var result = await _service.GetTestimonialByIdAsync(id);

            Assert.Equal(dto, result);
            Assert.Equal("Test", result.NameSurname);
        }

        #endregion

        #region UPDATE

        [Fact]
        public async Task UpdateTestimonialAsync_Should_Call_Repository()
        {
            var dto = new UpdateTestimonialDto
            {
                Id = "507f1f77bcf86cd799439011",
                NameSurname = "Updated"
            };

            _mockMapper.Setup(x => x.Map<Testimonial>(dto))
                .Returns(new Testimonial { Id = dto.Id });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Testimonial>()))
                .Returns(Task.CompletedTask);

            await _service.UpdateTestimonialAsync(dto);

            _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Testimonial>()), Times.Once);
        }

        #endregion

        #region DELETE

        [Fact]
        public async Task DeleteTestimonialAsync_Should_Call_Repository()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockRepository.Setup(x => x.DeleteAsync(id))
                .Returns(Task.CompletedTask);

            await _service.DeleteTestimonialAsync(id);

            _mockRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteTestimonialAsync_Should_Send_Correct_Id()
        {
            var id = "507f1f77bcf86cd799439011";
            string captured = null;

            _mockRepository.Setup(x => x.DeleteAsync(It.IsAny<string>()))
                .Callback<string>(x => captured = x)
                .Returns(Task.CompletedTask);

            await _service.DeleteTestimonialAsync(id);

            Assert.Equal(id, captured);
        }

        #endregion
    }
}