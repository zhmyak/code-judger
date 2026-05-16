using Microsoft.EntityFrameworkCore;
using OnlineJudger.Domain.Entities;
using OnlineJudger.Domain.Stores;

namespace OnlineJudger.Infrastructure.Persistance
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly OnlineJudgeContext _context;

        public SubmissionRepository(OnlineJudgeContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Submission submission)
        {
            await _context.Submissions.AddAsync(submission);
        }

        public async Task<IReadOnlyList<Submission>> GetAllAsync()
        {
            return await _context.Submissions.AsNoTracking().ToListAsync();
        }

        public async Task<Submission?> GetByIdAsync(int id)
        {
            return await _context.Submissions.FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<IReadOnlyList<Submission>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Submissions.Where(s => s.UserId == userId).ToListAsync();
        }
        public async Task<Submission?> GetByIdWithLanguageAsync(int id)
        {
            return await _context.Submissions
                .Include(s => s.Language)
                .FirstOrDefaultAsync(s => s.Id == id);

        }

        public void Update(Submission submission)
        {
            _context.Submissions.Update(submission);
        }

        public Task UpdateErrorMessageAsync(int id, string message)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStatusAsync(int id, string status)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Submission>> GetAllByUserAndProblemIdAsync(int userId, int problemId)
        {
            return await _context.Submissions.Where(s => s.UserId == userId && s.ProblemId == problemId).ToListAsync();
        }
    }
}
