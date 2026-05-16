using OnlineJudger.Domain.Entities;

namespace OnlineJudger.JudgeWorker.Interfaces
{
    public interface IScoreCounter
    {
        Task AddScore(Submission submission);
    }
}
