using OnlineJudger.Domain.Entities;
using OnlineJudger.Domain.Stores;
using OnlineJudger.JudgeWorker.Interfaces;

namespace OnlineJudger.JudgeWorker.Services
{
    public class ScoreCounter : IScoreCounter
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ScoreCounter(
            ISubmissionRepository submissionRepository,
            IUserRepository userRepository,
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork)
        {
            _submissionRepository = submissionRepository;
            _userRepository = userRepository;
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task AddScore(Submission sub)
        {
            var submissionsByUser = await _submissionRepository.GetAllByUserIdAsync(sub.UserId);
            var isSolved = submissionsByUser.Any(s => s.ProblemId == sub.ProblemId && s.Status == Domain.Enums.SubmissionStatus.Accepted);
            if (isSolved)
            {
                return;
            }
            var problem = await _problemRepository.GetByIdAsync(sub.ProblemId);
            if (problem == null)
            {
                throw new ArgumentException(nameof(_problemRepository.GetByIdAsync));
            }
            int pointsToAdd = problem.Difficulty switch
            {
                Domain.Enums.Difficulty.Easy => 10,
                Domain.Enums.Difficulty.Medium => 30,
                Domain.Enums.Difficulty.Hard => 50,
                _ => throw new Exception($"Неизвестная сложность {problem.Difficulty}")
            };
            var user = await _userRepository.GetByIdAsync(sub.UserId);
            if (user == null)
            {
                throw new ArgumentException(nameof(_userRepository.GetByIdAsync));
            }
            user.Points += pointsToAdd;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
