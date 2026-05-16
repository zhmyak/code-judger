using OnlineJudger.Domain.Entities;
using OnlineJudger.JudgeWorker.Models;

namespace OnlineJudger.JudgeWorker.Interfaces
{
    public interface ITestRunner
    {
        Task<TestRunResult> RunTestAsync(string runCommand, string path, TestCase testCase, string methodName);
    }
}
