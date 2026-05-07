using OnlineJudger.Domain.Entities;
namespace OnlineJudger.Domain.Stores
{
    public interface IProblemRepository
    {
        Task<string> GetMethodNameById(int id);
        Task<Problem?> GetByIdAsync(int id);
        Task<List<Problem>> GetAllAsync();
    }
}
