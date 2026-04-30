using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.HowItWorkDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.HowItWorkServices;

public class HowItWorkService : IHowItWorkService
{
    private readonly IGenericRepository<HowItWork> _repository;
    private readonly IMapper _mapper;

    public HowItWorkService(IGenericRepository<HowItWork> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task CreateHowItWorkAsync(CreateHowItWorkDto dto)
    {
        var entity = _mapper.Map<HowItWork>(dto);
        await _repository.CreateAsync(entity);
    }

    public async Task DeleteHowItWorkAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<ResultHowItWorkDto>> GetAllHowItWorkAsync()
    {
        var values = await _repository.GetAllAsync();
        return _mapper.Map<List<ResultHowItWorkDto>>(values);
    }

    public async Task<GetHowItWorkByIdDto> GetHowItWorkByIdAsync(string id)
    {
        var value = await _repository.GetByIdAsync(id);
        return _mapper.Map<GetHowItWorkByIdDto>(value);
    }

    public async Task UpdateHowItWorkAsync(UpdateHowItWorkDto dto)
    {
        var entity = _mapper.Map<HowItWork>(dto);
        await _repository.UpdateAsync(entity);
    }
}