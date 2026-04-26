namespace DatabaseMastery.TransportMongoDb.Dtos.BrandDtos
{
    public class ResultBrandDto
    {
        public string Id { get; set; }
        public string? BrandName { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsStatus { get; set; }
    }
}
