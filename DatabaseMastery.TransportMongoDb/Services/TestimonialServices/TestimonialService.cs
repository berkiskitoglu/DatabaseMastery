using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.TestimonialDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.TestimonialServices;

public class TestimonialService : ITestimonialService
{
    private readonly IGenericRepository<Testimonial> _repository;
    private readonly IMapper _mapper;

    public TestimonialService(IGenericRepository<Testimonial> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task CreateTestimonialAsync(CreateTestimonialDto dto)
    {
        var entity = _mapper.Map<Testimonial>(dto);
        await _repository.CreateAsync(entity);
    }

    public async Task DeleteTestimonialAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<ResultTestimonialDto>> GetAllTestimonialAsync()
    {
        var values = await _repository.GetAllAsync();
        return _mapper.Map<List<ResultTestimonialDto>>(values);
    }

    public async Task<GetTestimonialByIdDto> GetTestimonialByIdAsync(string id)
    {
        var value = await _repository.GetByIdAsync(id);
        return _mapper.Map<GetTestimonialByIdDto>(value);
    }

    public async Task UpdateTestimonialAsync(UpdateTestimonialDto dto)
    {
        var entity = _mapper.Map<Testimonial>(dto);
        await _repository.UpdateAsync(entity);
    }
}