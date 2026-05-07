
using OnlineJudger.Domain.Stores;
using OnlineJudger.JudgeWorker.Interfaces;

namespace OnlineJudger.JudgeWorker
{
    public class JudgeEngine
    {
        ITestRunner _testRunner;
        ITestCaseRepository _testCaseRepo;
        ISubmissionRepository _submissionRepo;
        IProblemRepository _problemRepo;
        IScoreCounter _scoreCounter;
        IUnitOfWork _unitOfWork;
        public JudgeEngine(
            ITestRunner testRunner, 
            ISubmissionRepository submissionRepository, 
            ITestCaseRepository testCaseRepository,
            IProblemRepository problemRepository,
            IScoreCounter scoreCounter,
            IUnitOfWork unitOfWork)
        {
            _testRunner = testRunner;
            _submissionRepo = submissionRepository;
            _problemRepo = problemRepository;
            _testCaseRepo = testCaseRepository;
            _scoreCounter = scoreCounter;
            _unitOfWork = unitOfWork;
        }
        public async Task ProcessAsync(int subId)
        {   
            var submission = await _submissionRepo.GetByIdWithLanguageAsync(subId);
            if (submission == null)
            {
                throw new Exception($"Нет Submission с id {subId}");
            }
            var testCasesList = await _testCaseRepo.GettAllByProblemAsync(submission.ProblemId);
            if (submission.Language.CompileCommand != null)
            {

            }
            string methodName = await _problemRepo.GetMethodNameById(submission.ProblemId);
            foreach (var testCase in testCasesList)
            {
                var result = await _testRunner.RunTestAsync(submission, testCase, methodName);
                string status = result.Status.ToString();
                if (result.Success == false)
                {
                    submission.Status = result.Status;
                    submission.ErrorMessage = result.ErrorMessage;
                    
                    await _unitOfWork.SaveChangesAsync();
                    return;
                }
            }
            await _scoreCounter.AddScore(submission);
            submission.Status = Domain.Enums.SubmissionStatus.Accepted;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
