using System;

namespace OnlineJudger.Domain.Enums
{
    public enum SubmissionStatus
    {
        Pending,
        Running,
        Accepted,
        WrongAnswer,
        TimeLimit,
        MemoryLimit,
        RuntimeError,
        CompilationError
    }
}
