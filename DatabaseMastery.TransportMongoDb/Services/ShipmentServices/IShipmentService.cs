using DatabaseMastery.TransportMongoDb.Dtos.ShipmentDtos;

namespace DatabaseMastery.TransportMongoDb.Services.ShipmentServices
{
    public interface IShipmentService
    {
        Task<List<ResultShipmentDto>> GetAllShipmentAsync();
        Task CreateShipmentAsync(CreateShipmentDto createShipmentDto);
        Task UpdateShipmentAsync(UpdateShipmentDto updateShipmentDto);
        Task<GetShipmentByIdDto> GetShipmentByIdAsync(string id);
        Task DeleteShipmentAsync(string id);
        Task<GetShipmentByIdDto> GetShipmentByTrackingNumberAsync(string trackingNumber);
        Task<long> GetTotalShipmentCountAsync();
        Task<long> GetDeliveredShipmentCountAsync();
        Task<int> GetDistinctDestinationCityCountAsync();
        Task<long> GetInDistributionShipmentCountAsync();
    }
}
