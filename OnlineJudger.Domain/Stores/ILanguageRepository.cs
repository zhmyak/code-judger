using OnlineJudger.Domain.Entities;

namespace OnlineJudger.Domain.Stores
{
    public interface ILanguageRepository
    {
        Task<Language?> GetById(int id);
        Task<List<Language>> GetAllAsync();
    }
}
