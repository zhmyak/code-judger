namespace OnlineJudger.Domain.Entities
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string? CompileCommand { get; set; }
        public string RunCommand { get; set; } = null!;
        public string DockerImage { get; set; } = null!;
        public List<Submission> Submissions { get; set; } = new List<Submission>();
        public List<CodeSnippet> CodeSnippets { get; set; } = new List<CodeSnippet>();
    }
}
