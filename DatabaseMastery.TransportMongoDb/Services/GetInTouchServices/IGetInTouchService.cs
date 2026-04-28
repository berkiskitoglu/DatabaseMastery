using DatabaseMastery.TransportMongoDb.Dtos.GetInTouchDtos;

namespace DatabaseMastery.TransportMongoDb.Services.GetInTouchServices
{
    public interface IGetInTouchService
    {
        Task<List<ResultGetInTouchDto>> GetAllGetInTouchAsync();
        Task CreateGetInTouchAsync(CreateGetInTouchDto createGetInTouchDto);
        Task UpdateGetInTouchAsync(UpdateGetInTouchDto updateGetInTouchDto);
        Task<GetInTouchByIdDto> GetInTouchByIdAsync(string id);
        Task DeleteGetInTouchAsync(string id);
    }
}
