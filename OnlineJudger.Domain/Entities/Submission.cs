using OnlineJudger.Domain.Enums;
namespace OnlineJudger.Domain.Entities
{
    public class Submission
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProblemId {  get; set; }
        public int LanguageId { get; set;}
        public string SourceCode { get; set; } = null!;
        public SubmissionStatus Status { get; set; }
        public int? ExecutionTimeMs {  get; set; }
        public int? MemoryUsedKb {  get; set; }
        public string? ErrorMessage { get; set;}
        public DateTime CreatedAt {  get; set; }
        public DateTime UpdatedAt { get; set; }

        public User User { get; set; } = null!;
        public Problem Problem { get; set; } = null!;
        public Language Language { get; set; } = null!;
        
    }
}
