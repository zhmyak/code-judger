using OnlineJudger.Domain.Entities;
using OnlineJudger.JudgeWorker.Models;

namespace OnlineJudger.JudgeWorker.Interfaces
{
    public interface ICompiler
    {
        Task<CompilationResult> CompileAsync(Language language, string path);
    }
}
