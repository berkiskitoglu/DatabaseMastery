using DatabaseMastery.TransportMongoDb.Dtos.SliderDtos;

namespace DatabaseMastery.TransportMongoDb.Services.SliderServices
{
    public interface ISliderService
    {
        Task<List<ResultSliderDto>> GetAllSliderAsync();
        Task CreateSliderAsync(CreateSliderDto createSliderDto);
        Task UpdateSliderAsync(UpdateSliderDto updateSliderDto);
        Task<GetSliderByIdDto> GetSliderByIdAsync(string id);
        Task DeleteSliderAsync(string id);
    }
}
