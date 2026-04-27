namespace DatabaseMastery.TransportMongoDb.Dtos.AboutDtos
{
    public class GetAboutByIdDto
    {
        public string Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; } 
        public string? ImageUrl { get; set; }
    }
}
