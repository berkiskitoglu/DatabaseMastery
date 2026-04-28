using DatabaseMastery.TransportMongoDb.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseMastery.TransportMongoDb.Entities
{
    public class GetInTouch : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string BadgeTitle { get; set; } = string.Empty;        
        public string MainTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Feature1Title { get; set; } = string.Empty;
        public string Feature1Description { get; set; } = string.Empty;
        public string Feature2Title { get; set; } = string.Empty;
        public string Feature2Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
