namespace OnlineJudger.API.Contracts
{
    public class SubmissionStatusResponse
    {
        public string Status { get; set; } = null!;
        public string? ErrorMessage { get; set; }
    }
}
