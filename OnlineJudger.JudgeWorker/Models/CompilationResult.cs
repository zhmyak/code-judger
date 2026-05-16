namespace OnlineJudger.JudgeWorker.Models
{
    public class CompilationResult
    {
        public bool IsSuccess { get; private set; }
        public string? BinaryPath { get; private set; }
        public string? ErrorMessage { get; private set; }

        public static CompilationResult Success(string binaryPath)
        {
            return new CompilationResult
            {
                IsSuccess = true,
                BinaryPath = binaryPath
            };
        }
        public static CompilationResult Failure(string errorMessage)
        {
            return new CompilationResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
