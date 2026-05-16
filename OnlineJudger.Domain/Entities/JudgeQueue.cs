using OnlineJudger.Domain.Enums;
namespace OnlineJudger.Domain.Entities
{
    public class JudgeQueue
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public SubmissionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public Submission Submission { get; set; } = null!;
    }
}
