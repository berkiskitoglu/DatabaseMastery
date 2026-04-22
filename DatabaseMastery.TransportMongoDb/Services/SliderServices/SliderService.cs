using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.SliderDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.SliderServices;

public class SliderService : ISliderService
{
    private readonly IGenericRepository<Slider> _repository;
    private readonly IMapper _mapper;

    public SliderService(IGenericRepository<Slider> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task CreateSliderAsync(CreateSliderDto dto)
    {
        var entity = _mapper.Map<Slider>(dto);
        await _repository.CreateAsync(entity);
    }

    public async Task DeleteSliderAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<ResultSliderDto>> GetAllSliderAsync()
    {
        var values = await _repository.GetAllAsync();
        return _mapper.Map<List<ResultSliderDto>>(values);
    }

    public async Task<GetSliderByIdDto> GetSliderByIdAsync(string id)
    {
        var value = await _repository.GetByIdAsync(id);
        return _mapper.Map<GetSliderByIdDto>(value);
    }

    public async Task UpdateSliderAsync(UpdateSliderDto dto)
    {
        var entity = _mapper.Map<Slider>(dto);
        await _repository.UpdateAsync(entity);
    }
}