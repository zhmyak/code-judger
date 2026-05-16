using OnlineJudger.Application.DTOs;
using OnlineJudger.Application.Exceptions;
using OnlineJudger.Domain.Entities;
using OnlineJudger.Domain.Enums;
using OnlineJudger.Domain.Stores;

namespace OnlineJudger.Application.Services
{
    public class SubmissionService
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IQueueRepository _queueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubmissionService(ISubmissionRepository submissionRepository, IQueueRepository queueRepository, IUnitOfWork unitOfWork)
        {
            _submissionRepository = submissionRepository;
            _unitOfWork = unitOfWork;
            _queueRepository = queueRepository;
        }
        public async Task<int> AddAsync(int userId, int problemId, int languageId, string sourceCode)
        {
            var submission = new Submission()
            {
                UserId = userId,
                ProblemId = problemId,
                LanguageId = languageId,
                SourceCode = sourceCode,
                Status = SubmissionStatus.Pending,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            var judgeQueueSub = new JudgeQueue
            {
                Submission = submission,
                Status = SubmissionStatus.Pending,
                CreatedAt = DateTime.Now
            };
            await _queueRepository.AddAsync(judgeQueueSub);
            await _unitOfWork.SaveChangesAsync();
            return submission.Id;
        }
        public async Task<SubmissionInfo> GetSubmissionInfo(int id)
        {
            var submission = await _submissionRepository.GetByIdAsync(id);
            if (submission == null)
            {
                throw new SubmissionNotFoundException();
            }
            return new SubmissionInfo
            {
                Status = submission.Status.ToString(),
                ErrorMessage = submission.ErrorMessage
            };
        }
        public async Task<Submission> GetByIdAsync(int id)
        {
            var submission = await _submissionRepository.GetByIdAsync(id);
            if (submission == null)
            {
                throw new SubmissionNotFoundException();
            }
            return submission;
        }
        public async Task<IReadOnlyList<Submission>> GetAllByUserIdAsync(int userId)
        {
            var submissions = await _submissionRepository.GetAllByUserIdAsync(userId);
            return submissions;
        }
    }
}
