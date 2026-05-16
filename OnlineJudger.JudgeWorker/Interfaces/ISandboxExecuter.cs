using OnlineJudger.JudgeWorker.Models;

namespace OnlineJudger.JudgeWorker.Interfaces
{
    public interface ISandboxExecuter
    {
        Task<ExecutionResult> ExecuteAsync(ExecutionOptions options);
    }
}
