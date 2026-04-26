using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.OfferDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.OfferServices;

public class OfferService : IOfferService
{
    private readonly IGenericRepository<Offer> _repository;
    private readonly IMapper _mapper;

    public OfferService(IGenericRepository<Offer> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task CreateOfferAsync(CreateOfferDto dto)
    {
        var entity = _mapper.Map<Offer>(dto);
        await _repository.CreateAsync(entity);
    }

    public async Task DeleteOfferAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<ResultOfferDto>> GetAllOfferAsync()
    {
        var values = await _repository.GetAllAsync();
        return _mapper.Map<List<ResultOfferDto>>(values);
    }

    public async Task<GetOfferByIdDto> GetOfferByIdAsync(string id)
    {
        var value = await _repository.GetByIdAsync(id);
        return _mapper.Map<GetOfferByIdDto>(value);
    }

    public async Task UpdateOfferAsync(UpdateOfferDto dto)
    {
        var entity = _mapper.Map<Offer>(dto);
        await _repository.UpdateAsync(entity);
    }
}