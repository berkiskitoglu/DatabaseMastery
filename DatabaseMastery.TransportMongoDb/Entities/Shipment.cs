using DatabaseMastery.TransportMongoDb.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseMastery.TransportMongoDb.Entities
{
    public class Shipment : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string TrackingNumber { get; set; } = string.Empty;

        public string SenderName { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;

        public string OriginCity { get; set; } = string.Empty;
        public string OriginDistrict { get; set; } = string.Empty;
        public string DestinationCity { get; set; } = string.Empty;
        public string DestinationDistrict { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string CurrentStatus { get; set; } = string.Empty;
        public List<ShipmentTracking> Trackings { get; set; } = new();
    }
}
