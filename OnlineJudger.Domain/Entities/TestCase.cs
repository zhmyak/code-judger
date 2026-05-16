namespace OnlineJudger.Domain.Entities
{
    public class TestCase
    {
        public int Id { get; set; }
        public int ProblemId { get; set; }
        public string InputData { get; set; } = null!;
        public string ExpectedOutput { get; set; } = null!;
        public Problem Problem { get; set; } = null!;
    }
}
