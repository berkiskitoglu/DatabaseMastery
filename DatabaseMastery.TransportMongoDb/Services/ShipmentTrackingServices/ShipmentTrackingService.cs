using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.ShipmentTrackingDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using MongoDB.Driver;

namespace DatabaseMastery.TransportMongoDb.Services.ShipmentTrackingServices
{
    public class ShipmentTrackingService : IShipmentTrackingService
    {
        private readonly IGenericRepository<Shipment> _repository;
        private readonly IMapper _mapper;

        public ShipmentTrackingService(IGenericRepository<Shipment> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateTrackingAsync(CreateShipmentTrackingDto createDto)
        {
            var tracking = _mapper.Map<ShipmentTracking>(createDto);
            var filter = Builders<Shipment>.Filter.Eq(x => x.TrackingNumber, createDto.TrackingNumber);
            var update = Builders<Shipment>.Update
                .Push(x => x.Trackings, tracking)
                .Set(x => x.CurrentStatus, createDto.TrackingStatus);
            await _repository.UpdateByFilterAsync(filter, update);
        }

        public async Task DeleteTrackingAsync(string trackingNumber, int index)
        {
            var filter = Builders<Shipment>.Filter.Eq(x => x.TrackingNumber, trackingNumber);
            var shipment = await _repository.GetByFilterAsync(filter);
            if (shipment == null || shipment.Trackings == null || index >= shipment.Trackings.Count)
                return;

            shipment.Trackings.RemoveAt(index);
            var update = Builders<Shipment>.Update.Set(x => x.Trackings, shipment.Trackings);
            await _repository.UpdateByFilterAsync(filter, update);
        }

        public async Task<List<ResultShipmentTrackingDto>> GetAllTrackingsAsync(string trackingNumber)
        {
            var filter = Builders<Shipment>.Filter.Eq(x => x.TrackingNumber, trackingNumber);
            var shipment = await _repository.GetByFilterAsync(filter);
            if (shipment == null || shipment.Trackings == null)
                return new List<ResultShipmentTrackingDto>();

            return _mapper.Map<List<ResultShipmentTrackingDto>>(shipment.Trackings);
        }

        public async Task<ResultShipmentTrackingDto> GetTrackingByIndexAsync(string trackingNumber, int index)
        {
            var filter = Builders<Shipment>.Filter.Eq(x => x.TrackingNumber, trackingNumber);
            var shipment = await _repository.GetByFilterAsync(filter);
            if (shipment == null || shipment.Trackings == null || index >= shipment.Trackings.Count)
                return null;

            return _mapper.Map<ResultShipmentTrackingDto>(shipment.Trackings[index]);
        }

        public async Task UpdateTrackingAsync(UpdateShipmentTrackingDto updateDto)
        {
            var filter = Builders<Shipment>.Filter.Eq(x => x.TrackingNumber, updateDto.TrackingNumber);
            var update = Builders<Shipment>.Update
                .Set($"Trackings.{updateDto.TrackingIndex}.EventDate", updateDto.EventDate)
                .Set($"Trackings.{updateDto.TrackingIndex}.Location", updateDto.Location)
                .Set($"Trackings.{updateDto.TrackingIndex}.Description", updateDto.Description)
                .Set($"Trackings.{updateDto.TrackingIndex}.TrackingStatus", updateDto.TrackingStatus)
                .Set(x => x.CurrentStatus, updateDto.TrackingStatus);
            await _repository.UpdateByFilterAsync(filter, update);
        }
    }
}