using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.BrandDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.BrandServices;

public class BrandService : IBrandService
{
    private readonly IGenericRepository<Brand> _repository;
    private readonly IMapper _mapper;

    public BrandService(IGenericRepository<Brand> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task CreateBrandAsync(CreateBrandDto dto)
    {
        var entity = _mapper.Map<Brand>(dto);
        await _repository.CreateAsync(entity);
    }

    public async Task DeleteBrandAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<ResultBrandDto>> GetAllBrandAsync()
    {
        var values = await _repository.GetAllAsync();
        return _mapper.Map<List<ResultBrandDto>>(values);
    }

    public async Task<GetBrandByIdDto> GetBrandByIdAsync(string id)
    {
        var value = await _repository.GetByIdAsync(id);
        return _mapper.Map<GetBrandByIdDto>(value);
    }

    public async Task UpdateBrandAsync(UpdateBrandDto dto)
    {
        var entity = _mapper.Map<Brand>(dto);
        await _repository.UpdateAsync(entity);
    }
}