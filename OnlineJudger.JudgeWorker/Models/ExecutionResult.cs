using System;

namespace OnlineJudger.JudgeWorker.Models
{
    public class ExecutionResult
    {
        public bool Success { get; set; }
        public string Output {  get; set; }
        public string ErrorMessage {  get; set; }
        public bool IsTimeout {  get; private set; }
        public bool IsMemoryOut {  get; private set; }
        public bool IsRuntimeError { get; private set; }


        public static ExecutionResult Timeout()
        {
            return new ExecutionResult
            {
                Success = false,
                IsTimeout = true
            };
        }
        public static ExecutionResult RuntimeError(string errMes)
        {
            return new ExecutionResult
            {
                Success = false,
                IsRuntimeError = true,
                ErrorMessage = errMes
            };
        }
        public static ExecutionResult MemoryOut()
        {
            return new ExecutionResult
            {
                Success = false,
                IsMemoryOut = true
            };
        }
    }
}
