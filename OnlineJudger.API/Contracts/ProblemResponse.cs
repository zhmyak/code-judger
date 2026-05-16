namespace OnlineJudger.API.Contracts
{
    public class ProblemResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Difficulty { get; set; }
        public bool IsSolved { get; set; }
    }
}
