namespace DatabaseMastery.TransportMongoDb.Dtos.HowItWorkDtos
{
    public class UpdateHowItWorkDto
    {
        public string Id { get; set; }
        public string? Title { get; set; } 
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public bool Status { get; set; }
    }
}
