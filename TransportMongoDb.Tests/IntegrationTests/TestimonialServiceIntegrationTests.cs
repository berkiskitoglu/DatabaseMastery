using DotNetEnv;
using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.TestimonialDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.TestimonialServices;
using MongoDB.Driver;
using Xunit;

namespace TransportMongoDb.Tests.IntegrationTests
{
    public class TestimonialServiceIntegrationTests : IAsyncLifetime
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _database;
        private IGenericRepository<Testimonial> _repository;
        private IMapper _mapper;
        private ITestimonialService _testimonialService;
        private string _connectionString;
        private readonly string _testDatabaseName;

        public TestimonialServiceIntegrationTests()
        {
            var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var binPath = Path.GetDirectoryName(assemblyLocation);
            var testProjectPath = Directory.GetParent(binPath)?.Parent?.Parent?.FullName;
            var solutionFolder = Directory.GetParent(testProjectPath)?.FullName;

            var envPath = Path.Combine(solutionFolder ?? "", ".env");

            if (!File.Exists(envPath))
                throw new Exception($".env file not found at {envPath}");

            Env.Load(envPath);

            _testDatabaseName = "test_" + Guid.NewGuid().ToString("N")[..8];

            _connectionString =
                Environment.GetEnvironmentVariable("DatabaseSettings__ConnectionString")
                ?? throw new Exception("MongoDB connection string missing");
        }

        public Task InitializeAsync()
        {
            _mongoClient = new MongoClient(_connectionString);
            _database = _mongoClient.GetDatabase(_testDatabaseName);

            var collection = _database.GetCollection<Testimonial>("Testimonials");
            _repository = new GenericRepository<Testimonial>(collection);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateTestimonialDto, Testimonial>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore());

                cfg.CreateMap<UpdateTestimonialDto, Testimonial>();
                cfg.CreateMap<Testimonial, ResultTestimonialDto>();
                cfg.CreateMap<Testimonial, GetTestimonialByIdDto>();
            });

            _mapper = config.CreateMapper();
            _testimonialService = new TestimonialService(_repository, _mapper);

            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            if (_mongoClient != null)
            {
                await _mongoClient.DropDatabaseAsync(_testDatabaseName);
                _mongoClient.Dispose();
            }
        }

        #region CREATE
        [Fact]
        public async Task CreateTestimonialAsync_Should_Save()
        {
            var dto = new CreateTestimonialDto
            {
                NameSurname = "John Doe",
                Title = "CEO",
                ImageUrl = "img.jpg",
                ReviewDetail = "Great service",
                ReviewScore = 5,
                Status = true
            };

            await _testimonialService.CreateTestimonialAsync(dto);

            var all = await _repository.GetAllAsync();
            Assert.Single(all);
            Assert.Equal("John Doe", all[0].NameSurname);
        }
        #endregion

        #region READ
        [Fact]
        public async Task GetAllTestimonialAsync_Should_Return_Empty()
        {
            var result = await _testimonialService.GetAllTestimonialAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllTestimonialAsync_Should_Return_All()
        {
            await _testimonialService.CreateTestimonialAsync(new CreateTestimonialDto
            {
                NameSurname = "A",
                ImageUrl = "a.jpg"
            });

            await _testimonialService.CreateTestimonialAsync(new CreateTestimonialDto
            {
                NameSurname = "B",
                ImageUrl = "b.jpg"
            });

            var result = await _testimonialService.GetAllTestimonialAsync();

            Assert.Equal(2, result.Count);
        }
        #endregion

        #region UPDATE
        [Fact]
        public async Task UpdateTestimonialAsync_Should_Update()
        {
            await _testimonialService.CreateTestimonialAsync(new CreateTestimonialDto
            {
                NameSurname = "Old Name",
                ImageUrl = "old.jpg"
            });

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            var updateDto = new UpdateTestimonialDto
            {
                Id = id,
                NameSurname = "New Name",
                ImageUrl = "new.jpg"
            };

            await _testimonialService.UpdateTestimonialAsync(updateDto);

            var updated = await _repository.GetByIdAsync(id);
            Assert.Equal("New Name", updated.NameSurname);
        }
        #endregion

        #region DELETE
        [Fact]
        public async Task DeleteTestimonialAsync_Should_Remove()
        {
            await _testimonialService.CreateTestimonialAsync(new CreateTestimonialDto
            {
                NameSurname = "Delete Me",
                ImageUrl = "del.jpg"
            });

            var all = await _repository.GetAllAsync();
            var id = all[0].Id;

            await _testimonialService.DeleteTestimonialAsync(id);

            var remaining = await _repository.GetAllAsync();
            Assert.Empty(remaining);
        }
        #endregion
    }
}