using OnlineJudger.Domain.Entities;

namespace OnlineJudger.Domain.Stores
{
    public interface IQueueRepository
    {
        Task<IReadOnlyList<JudgeQueue>> GetAllAsync();
        Task<JudgeQueue?> GetFirstAsync();
        Task<JudgeQueue?> GetByIdAsync(int id);
        Task AddAsync(JudgeQueue judgeQueue);
        void Remove(JudgeQueue judgeQueue);
        void Update(JudgeQueue judgeQueue);
    }
}
