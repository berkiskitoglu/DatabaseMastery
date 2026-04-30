using DatabaseMastery.TransportMongoDb.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseMastery.TransportMongoDb.Entities
{
    public class Testimonial : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string NameSurname { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ReviewDetail { get; set; } = string.Empty;
        public int ReviewScore { get; set; }
        public bool Status { get; set; } 
    }
}
