using OnlineJudger.Domain.Entities;

namespace OnlineJudger.Domain.Stores
{
    public interface ITestCaseRepository
    {
        Task<IReadOnlyList<TestCase>> GetAllAsync();
        Task<IReadOnlyList<TestCase>> GettAllByProblemAsync(int problemID);
        Task<TestCase?> GetByIdAsync(int id);

    }
}
