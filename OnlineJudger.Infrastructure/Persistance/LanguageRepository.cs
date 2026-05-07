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
    public class LanguageRepository : ILanguageRepository
    {
        private readonly OnlineJudgeContext _context;

        public LanguageRepository(OnlineJudgeContext context)
        {
            _context = context;
        }

        public async Task<List<Language>> GetAllAsync()
        {
            return await _context.Languages.ToListAsync();
        }

        public async Task<Language?> GetById(int id)
        {
            return await _context.Languages.FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
