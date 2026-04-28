using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.GetInTouchDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.GetInTouchServices;

public class GetInTouchService : IGetInTouchService
{
    private readonly IGenericRepository<GetInTouch> _repository;
    private readonly IMapper _mapper;

    public GetInTouchService(IGenericRepository<GetInTouch> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task CreateGetInTouchAsync(CreateGetInTouchDto dto)
    {
        var entity = _mapper.Map<GetInTouch>(dto);
        await _repository.CreateAsync(entity);
    }

    public async Task DeleteGetInTouchAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<ResultGetInTouchDto>> GetAllGetInTouchAsync()
    {
        var values = await _repository.GetAllAsync();
        return _mapper.Map<List<ResultGetInTouchDto>>(values);
    }

    public async Task<GetInTouchByIdDto> GetInTouchByIdAsync(string id)
    {
        var value = await _repository.GetByIdAsync(id);
        return _mapper.Map<GetInTouchByIdDto>(value);
    }

    public async Task UpdateGetInTouchAsync(UpdateGetInTouchDto dto)
    {
        var entity = _mapper.Map<GetInTouch>(dto);
        await _repository.UpdateAsync(entity);
    }
}