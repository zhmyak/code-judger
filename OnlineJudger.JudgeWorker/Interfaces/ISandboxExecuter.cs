using OnlineJudger.Domain.Entities;
using OnlineJudger.JudgeWorker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineJudger.JudgeWorker.Interfaces
{
    public interface ISandboxExecuter
    {
        Task<ExecutionResult> ExecuteAsync(ExecutionOptions options);
    }
}
