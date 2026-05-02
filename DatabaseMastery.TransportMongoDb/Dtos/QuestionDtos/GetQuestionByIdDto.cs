namespace DatabaseMastery.TransportMongoDb.Dtos.QuestionDtos
{
    public class GetQuestionByIdDto
    {
        public string Id { get; set; }
        public string? Title { get; set; } 
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
