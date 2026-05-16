namespace OnlineJudger.Application.DTOs
{
    public class ProblemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Difficulty { get; set; }
        public bool IsSolved { get; set; }
    }
}
