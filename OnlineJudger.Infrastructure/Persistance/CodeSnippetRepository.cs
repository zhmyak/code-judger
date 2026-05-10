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
    public class CodeSnippetRepository : ICodeSnippetRepository
    {
        private readonly OnlineJudgeContext _context;

        public CodeSnippetRepository(OnlineJudgeContext context)
        {
            _context = context;
        }

        public async Task<CodeSnippet?> GetByIdAsync(int problemId, int languageId)
        {
            return await _context.CodeSnippets
                 .FirstOrDefaultAsync(cs => cs.ProblemId == problemId && cs.LanguageId == languageId);
     
        }
    }
}
