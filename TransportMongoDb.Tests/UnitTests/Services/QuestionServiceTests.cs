using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.QuestionDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.QuestionServices;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TransportMongoDb.Tests.UnitTests.Services
{
    public class QuestionServiceTests
    {
        private readonly Mock<IGenericRepository<Question>> _mockRepository;
        private readonly QuestionService _questionService;
        private readonly Mock<IMapper> _mockMapper;

        public QuestionServiceTests()
        {
            _mockRepository = new Mock<IGenericRepository<Question>>();
            _mockMapper = new Mock<IMapper>();
            _questionService = new QuestionService(_mockRepository.Object, _mockMapper.Object);
        }

        #region CreateQuestionAsync Tests

        [Fact]
        public async Task CreateQuestionAsync_Should_Call_Repository_CreateAsync_Once()
        {
            var createDto = new CreateQuestionDto
            {
                Title = "Question",
                Description = "Desc",
                Status = true
            };

            _mockMapper.Setup(x => x.Map<Question>(createDto)).Returns(new Question
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Status = createDto.Status
            });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Question>()))
                           .Returns(Task.CompletedTask);

            await _questionService.CreateQuestionAsync(createDto);

            _mockRepository.Verify(x => x.CreateAsync(It.IsAny<Question>()), Times.Once);
        }

        [Fact]
        public async Task CreateQuestionAsync_Should_Pass_Correct_Data()
        {
            var createDto = new CreateQuestionDto
            {
                Title = "My Question",
                Description = "My Desc",
                Status = true
            };

            _mockMapper.Setup(x => x.Map<Question>(createDto)).Returns(new Question
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Status = createDto.Status
            });

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Question>()))
                           .Returns(Task.CompletedTask);

            await _questionService.CreateQuestionAsync(createDto);

            _mockRepository.Verify(x => x.CreateAsync(It.Is<Question>(q =>
                q.Title == "My Question" &&
                q.Description == "My Desc" &&
                q.Status == true
            )), Times.Once);
        }

        #endregion

        #region GetAllQuestionAsync Tests

        [Fact]
        public async Task GetAllQuestionAsync_Should_Return_List()
        {
            var questions = new List<Question>
            {
                new Question { Title = "Q1", Status = true }
            };

            var mapped = new List<ResultQuestionDto>
            {
                new ResultQuestionDto { Title = "Q1" }
            };

            _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(questions);
            _mockMapper.Setup(x => x.Map<List<ResultQuestionDto>>(questions)).Returns(mapped);

            var result = await _questionService.GetAllQuestionAsync();

            Assert.Single(result);
        }

        #endregion

        #region GetQuestionByIdAsync Tests

        [Fact]
        public async Task GetQuestionByIdAsync_Should_Return_Data()
        {
            var id = "507f1f77bcf86cd799439011";

            var entity = new Question
            {
                Id = id,
                Title = "Test"
            };

            var dto = new GetQuestionByIdDto
            {
                Title = "Test"
            };

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(entity);
            _mockMapper.Setup(x => x.Map<GetQuestionByIdDto>(entity)).Returns(dto);

            var result = await _questionService.GetQuestionByIdAsync(id);

            Assert.Equal("Test", result.Title);
        }

        #endregion

        #region UpdateQuestionAsync Tests

        [Fact]
        public async Task UpdateQuestionAsync_Should_Call_Update()
        {
            var updateDto = new UpdateQuestionDto
            {
                Id = "507f1f77bcf86cd799439011",
                Title = "Updated",
                Description = "Updated Desc",
                Status = false
            };

            _mockMapper.Setup(x => x.Map<Question>(updateDto)).Returns(new Question
            {
                Id = updateDto.Id,
                Title = updateDto.Title,
                Description = updateDto.Description,
                Status = updateDto.Status
            });

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Question>()))
                           .Returns(Task.CompletedTask);

            await _questionService.UpdateQuestionAsync(updateDto);

            _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Question>()), Times.Once);
        }

        #endregion

        #region DeleteQuestionAsync Tests

        [Fact]
        public async Task DeleteQuestionAsync_Should_Call_Delete()
        {
            var id = "507f1f77bcf86cd799439011";

            _mockRepository.Setup(x => x.DeleteAsync(id))
                           .Returns(Task.CompletedTask);

            await _questionService.DeleteQuestionAsync(id);

            _mockRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        #endregion
    }
}