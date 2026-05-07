using System.ComponentModel.DataAnnotations;

namespace DatabaseMastery.TransportMongoDb.Dtos.ShipmentTrackingDtos
{
    public class CreateShipmentTrackingDto
    {
        public string? TrackingNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? TrackingStatus { get; set; }
    }
}
