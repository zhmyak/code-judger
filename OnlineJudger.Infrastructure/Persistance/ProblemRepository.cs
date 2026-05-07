using Microsoft.EntityFrameworkCore;
using OnlineJudger.Domain.Entities;
using OnlineJudger.Domain.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineJudger.Infrastructure.Persistance
{
    public class ProblemRepository : IProblemRepository
    {
        private readonly OnlineJudgeContext _context;
        public ProblemRepository(OnlineJudgeContext context)
        {
            _context = context;
        }

        public async Task<Problem?> GetByIdAsync(int id)
        {
            return await _context.Problems
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<string> GetMethodNameById(int id)
        {
            return await _context.Problems
                .Where(p => p.Id == id)
                .Select(p => p.MethodName)
                .FirstAsync();
        }
        public async Task<List<Problem>> GetAllAsync()
        {
            return await _context.Problems
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
