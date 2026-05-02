namespace DatabaseMastery.TransportMongoDb.Dtos.QuestionDtos
{
    public class ResultQuestionDto
    {
        public string Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
