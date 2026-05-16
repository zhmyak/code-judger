namespace OnlineJudger.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int Points { get; set; } = 0;
        public List<Submission> Submissions { get; set; } = new();
    }
}
