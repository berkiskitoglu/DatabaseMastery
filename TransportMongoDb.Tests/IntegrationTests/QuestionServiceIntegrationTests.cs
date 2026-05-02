using DotNetEnv;
using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.QuestionDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.QuestionServices;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace TransportMongoDb.Tests.IntegrationTests
{
    public class QuestionServiceIntegrationTests : IAsyncLifetime
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _database;
        private IGenericRepository<Question> _repository;
        private IMapper _mapper;
        private IQuestionService _service;
        private string _connectionString;
        private readonly string _dbName;

        public QuestionServiceIntegrationTests()
        {
            var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var binPath = Path.GetDirectoryName(assemblyLocation);
            var testProjectPath = Directory.GetParent(binPath)?.Parent?.Parent?.FullName;
            var solutionFolder = Directory.GetParent(testProjectPath)?.FullName;

            var envPath = Path.Combine(solutionFolder ?? "", ".env");
            Env.Load(envPath);

            _dbName = "test_" + Guid.NewGuid().ToString("N")[..8];

            _connectionString =
                Environment.GetEnvironmentVariable("DatabaseSettings__ConnectionString")
                ?? throw new Exception("Mongo missing");
        }

        public Task InitializeAsync()
        {
            _mongoClient = new MongoClient(_connectionString);
            _database = _mongoClient.GetDatabase(_dbName);

            var collection = _database.GetCollection<Question>("Questions");

            _repository = new GenericRepository<Question>(collection);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateQuestionDto, Question>()
                    .ForMember(x => x.Id, opt => opt.Ignore());

                cfg.CreateMap<UpdateQuestionDto, Question>();
                cfg.CreateMap<Question, ResultQuestionDto>();
                cfg.CreateMap<Question, GetQuestionByIdDto>();
            });

            _mapper = config.CreateMapper();
            _service = new QuestionService(_repository, _mapper);

            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await _mongoClient.DropDatabaseAsync(_dbName);
        }

        #region CREATE

        [Fact]
        public async Task Create_Should_Work()
        {
            await _service.CreateQuestionAsync(new CreateQuestionDto
            {
                Title = "Q1",
                Description = "Desc",
                Status = true
            });

            var all = await _repository.GetAllAsync();
            Assert.Single(all);
        }

        #endregion

        #region READ

        [Fact]
        public async Task GetAll_Should_Return_Data()
        {
            await _service.CreateQuestionAsync(new CreateQuestionDto { Title = "Q1" });

            var result = await _service.GetAllQuestionAsync();

            Assert.Single(result);
        }

        #endregion

        #region UPDATE

        [Fact]
        public async Task Update_Should_Work()
        {
            await _service.CreateQuestionAsync(new CreateQuestionDto { Title = "Old" });

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            await _service.UpdateQuestionAsync(new UpdateQuestionDto
            {
                Id = id,
                Title = "New",
                Status = false
            });

            var updated = await _repository.GetByIdAsync(id);
            Assert.Equal("New", updated.Title);
        }

        #endregion

        #region DELETE

        [Fact]
        public async Task Delete_Should_Work()
        {
            await _service.CreateQuestionAsync(new CreateQuestionDto { Title = "Delete" });

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            await _service.DeleteQuestionAsync(id);

            var remaining = await _repository.GetAllAsync();
            Assert.Empty(remaining);
        }

        #endregion
    }
}