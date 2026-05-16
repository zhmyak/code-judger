using Microsoft.EntityFrameworkCore;
using OnlineJudger.Domain.Entities;
using OnlineJudger.Domain.Stores;

namespace OnlineJudger.Infrastructure.Persistance
{
    public class QueueRepository : IQueueRepository
    {
        private readonly OnlineJudgeContext _context;

        public QueueRepository(OnlineJudgeContext context)
        {
            _context = context;
        }

        public async Task AddAsync(JudgeQueue entity)
        {
            await _context.JudgeQueue.AddAsync(entity);
        }

        public void Remove(JudgeQueue entity)
        {
            _context.JudgeQueue.Remove(entity);
        }

        public async Task<IReadOnlyList<JudgeQueue>> GetAllAsync()
        {
            return await _context.JudgeQueue.AsNoTracking().ToListAsync();
        }

        public async Task<JudgeQueue?> GetByIdAsync(int id)
        {
            return await _context.JudgeQueue.AsNoTracking().FirstOrDefaultAsync(jq => jq.Id == id);
        }

        public async Task<JudgeQueue?> GetFirstAsync()
        {
            return await _context.JudgeQueue.AsNoTracking().FirstOrDefaultAsync();
        }

        public void Update(JudgeQueue judgeQueue)
        {
            _context.JudgeQueue.Update(judgeQueue);
        }
    }
}
