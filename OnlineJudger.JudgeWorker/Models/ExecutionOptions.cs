using System;

namespace OnlineJudger.JudgeWorker.Models
{
    public record ExecutionOptions(string RunCommand, string Path, string TestCaseJson);
}
