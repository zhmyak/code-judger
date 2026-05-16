using Microsoft.EntityFrameworkCore;
using OnlineJudger.Domain.Entities;
using OnlineJudger.Domain.Stores;

namespace OnlineJudger.Infrastructure.Persistance
{
    public class TestCaseRepository : ITestCaseRepository
    {
        private readonly OnlineJudgeContext _context;
        public TestCaseRepository(OnlineJudgeContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<TestCase>> GetAllAsync()
        {
            return await _context.TestCases.AsNoTracking().ToListAsync();
        }

        public async Task<TestCase?> GetByIdAsync(int id)
        {
            return await _context.TestCases.AsNoTracking().FirstOrDefaultAsync(tc => tc.Id == id);
        }

        public async Task<IReadOnlyList<TestCase>> GettAllByProblemAsync(int problemID)
        {
            return await _context.TestCases.AsNoTracking().Where(tc => tc.ProblemId == problemID).ToListAsync();
        }
    }
}
