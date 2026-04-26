using DatabaseMastery.TransportMongoDb.Dtos.OfferDtos;

namespace DatabaseMastery.TransportMongoDb.Services.OfferServices
{
    public interface IOfferService
    {
        Task<List<ResultOfferDto>> GetAllOfferAsync();
        Task CreateOfferAsync(CreateOfferDto createOfferDto);
        Task UpdateOfferAsync(UpdateOfferDto updateOfferDto);
        Task<GetOfferByIdDto> GetOfferByIdAsync(string id);
        Task DeleteOfferAsync(string id);
    }
}
