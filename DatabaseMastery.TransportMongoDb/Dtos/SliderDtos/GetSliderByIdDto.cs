using DatabaseMastery.TransportMongoDb.Entities;
using MongoDB.Bson;
using System.Diagnostics.CodeAnalysis;

namespace DatabaseMastery.TransportMongoDb.Dtos.SliderDtos
{
    public class GetSliderByIdDto
    {
        public string? Id { get; set; }
        public string? Title { get; set; } 
        public string? SubTitle { get; set; } 
        public string? Description { get; set; } 
        public string? ImageUrl { get; set; }
    }
}
