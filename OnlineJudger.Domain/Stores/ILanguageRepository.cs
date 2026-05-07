using OnlineJudger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineJudger.Domain.Stores
{
     public interface ILanguageRepository
    {
        Task<Language?> GetById(int id);
        Task<List<Language>> GetAllAsync();
    }
}
