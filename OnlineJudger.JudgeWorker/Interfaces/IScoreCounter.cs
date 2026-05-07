using OnlineJudger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineJudger.JudgeWorker.Interfaces
{
    public interface IScoreCounter
    {
        Task AddScore(Submission submission);
    }
}
