using MongoDB.Bson;

namespace DatabaseMastery.TransportMongoDb.Dtos.SliderDtos
{
    public class UpdateSliderDto
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
