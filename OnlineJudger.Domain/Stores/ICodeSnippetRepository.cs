using OnlineJudger.Domain.Entities;

namespace OnlineJudger.Domain.Stores
{
    public interface ICodeSnippetRepository
    {
        Task<CodeSnippet?> GetByIdAsync(int problemId, int languageId);

    }
}
