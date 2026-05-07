using OnlineJudger.Domain.Enums;
using System;

namespace OnlineJudger.JudgeWorker.Models
{
     public class TestRunResult
    {
        public bool Success { get; init; }
        public SubmissionStatus Status { get; init; }
        public string? ErrorMessage { get; init; }
        public string ExpectedOutput {  get; init; }
        public string ActualOutput {  get; init; }

       

    }
}
