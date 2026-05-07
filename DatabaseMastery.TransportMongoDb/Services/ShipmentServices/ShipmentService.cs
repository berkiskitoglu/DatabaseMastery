using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.ShipmentDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.ShipmentServices;
using MongoDB.Driver;

public class ShipmentService : IShipmentService
{
    private readonly IGenericRepository<Shipment> _repository;
    private readonly IMapper _mapper;

    public ShipmentService(IGenericRepository<Shipment> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task CreateShipmentAsync(CreateShipmentDto dto)
    {
        var entity = _mapper.Map<Shipment>(dto);
        await _repository.CreateAsync(entity);
    }

    public async Task DeleteShipmentAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<ResultShipmentDto>> GetAllShipmentAsync()
    {
        var values = await _repository.GetAllAsync();
        return _mapper.Map<List<ResultShipmentDto>>(values);
    }

    public async Task<GetShipmentByIdDto> GetShipmentByIdAsync(string id)
    {
        var value = await _repository.GetByIdAsync(id);
        return _mapper.Map<GetShipmentByIdDto>(value);
    }

    public async Task UpdateShipmentAsync(UpdateShipmentDto dto)
    {
        var entity = _mapper.Map<Shipment>(dto);
        await _repository.UpdateAsync(entity);
    }

    public async Task<GetShipmentByIdDto> GetShipmentByTrackingNumberAsync(string trackingNumber)
    {
        var filter = Builders<Shipment>.Filter.Eq(x => x.TrackingNumber, trackingNumber);
        var value = await _repository.GetByFilterAsync(filter);
        return _mapper.Map<GetShipmentByIdDto>(value);
    }

    public async Task<long> GetTotalShipmentCountAsync()
    {
        return await _repository.CountDocumentsAsync(FilterDefinition<Shipment>.Empty);
    }

    public async Task<long> GetDeliveredShipmentCountAsync()
    {
        var filter = Builders<Shipment>.Filter.Eq(x => x.CurrentStatus, "Teslim Edildi");
        return await _repository.CountDocumentsAsync(filter);
    }

    public async Task<long> GetInDistributionShipmentCountAsync()
    {
        var filter = Builders<Shipment>.Filter.Eq(x => x.CurrentStatus, "Dağıtımda");
        return await _repository.CountDocumentsAsync(filter);
    }

    public async Task<int> GetDistinctDestinationCityCountAsync()
    {
        var cities = await _repository.GetDistinctAsync<string>("DestinationCity", FilterDefinition<Shipment>.Empty);
        return cities.Count;
    }
}