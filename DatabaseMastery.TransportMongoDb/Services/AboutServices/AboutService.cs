using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.AboutDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.AboutServices;

public class AboutService : IAboutService
{
    private readonly IGenericRepository<About> _repository;
    private readonly IMapper _mapper;

    public AboutService(IGenericRepository<About> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task CreateAboutAsync(CreateAboutDto dto)
    {
        var entity = _mapper.Map<About>(dto);
        await _repository.CreateAsync(entity);
    }

    public async Task DeleteAboutAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<ResultAboutDto>> GetAllAboutAsync()
    {
        var values = await _repository.GetAllAsync();
        return _mapper.Map<List<ResultAboutDto>>(values);
    }

    public async Task<GetAboutByIdDto> GetAboutByIdAsync(string id)
    {
        var value = await _repository.GetByIdAsync(id);
        return _mapper.Map<GetAboutByIdDto>(value);
    }

    public async Task UpdateAboutAsync(UpdateAboutDto dto)
    {
        var entity = _mapper.Map<About>(dto);
        await _repository.UpdateAsync(entity);
    }
}