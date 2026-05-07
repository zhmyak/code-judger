using OnlineJudger.Domain.Enums;
namespace OnlineJudger.Domain.Entities
{
    public class Problem
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Difficulty Difficulty { get; set; }
        public int TimeLimitMs {  get; set; }
        public int MemoryLimitMb {  get; set; }
        public DateTime CreatedAt {  get; set; }
        public string MethodName { get; set; } = null!;

        public List<Submission> Submissions { get; set; } = new ();
        public List<TestCase> TestCases { get; set; } = new ();
        public List<CodeSnippet> CodeSnippets { get; set; } = new();
    }
}
