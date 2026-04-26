using DatabaseMastery.TransportMongoDb.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseMastery.TransportMongoDb.Entities
{
    public class Brand : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsStatus { get; set; } 
    }
}
