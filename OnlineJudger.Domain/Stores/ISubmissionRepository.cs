using OnlineJudger.Domain.Entities;

namespace OnlineJudger.Domain.Stores
{
    public interface ISubmissionRepository
    {
        Task<IReadOnlyList<Submission>> GetAllAsync();
        Task<Submission?> GetByIdAsync(int id);
        Task<IReadOnlyList<Submission>> GetAllByUserIdAsync(int userId);
        Task<Submission?> GetByIdWithLanguageAsync(int id);
        Task AddAsync(Submission submission);
        void Update(Submission submission);
        Task UpdateStatusAsync(int id, string status);
        Task UpdateErrorMessageAsync(int id, string message);

    }
}
