using OnlineJudger.Domain.Stores;
using OnlineJudger.JudgeWorker.Interfaces;
using OnlineJudger.JudgeWorker.Services;

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
        ICompiler _compiler;
        public JudgeEngine(
            ITestRunner testRunner,
            ISubmissionRepository submissionRepository,
            ITestCaseRepository testCaseRepository,
            IProblemRepository problemRepository,
            IScoreCounter scoreCounter,
            IUnitOfWork unitOfWork,
            ICompiler compiler)
        {
            _testRunner = testRunner;
            _submissionRepo = submissionRepository;
            _problemRepo = problemRepository;
            _testCaseRepo = testCaseRepository;
            _scoreCounter = scoreCounter;
            _unitOfWork = unitOfWork;
            _compiler = compiler;
        }
        public async Task ProcessAsync(int subId)
        {
            var submission = await _submissionRepo.GetByIdWithLanguageAsync(subId);
            if (submission == null)
            {
                throw new Exception($"Нет Submission с id {subId}");
            }
            var testCasesList = await _testCaseRepo.GettAllByProblemAsync(submission.ProblemId);
            var language = submission.Language;
            string sourceCode = submission.SourceCode;
            var workDir = Path.Combine("tmp/submissions", submission.Id.ToString());
            Directory.CreateDirectory(workDir);
            string fileName = language.FileName;
            string fullPath = Path.Combine(workDir, fileName);
            string harness = language.Name switch
            {
                "Python" => HarnessService.GetPythonHarness(),
                "Java" => HarnessService.GetJavaHarness(),
                "C#" => HarnessService.GetCSharpHarness(),
                _ => throw new Exception($"{language.Name} not implemented yet")
            };

            string fullCode = sourceCode + harness;
            if (language.CompileCommand != null)
                fullCode = harness + sourceCode;
            await File.WriteAllTextAsync(fullPath, fullCode);
            if (language.CompileCommand != null)
            {
                var compilationResult = await _compiler.CompileAsync(language, workDir);
                if (compilationResult.IsSuccess == false)
                {
                    submission.Status = Domain.Enums.SubmissionStatus.CompilationError;
                    submission.ErrorMessage = compilationResult.ErrorMessage;
                    await _unitOfWork.SaveChangesAsync();
                    return;
                }
                fullPath = compilationResult.BinaryPath!;
            }
            string methodName = await _problemRepo.GetMethodNameById(submission.ProblemId);
            foreach (var testCase in testCasesList)
            {
                var result = await _testRunner.RunTestAsync(language.RunCommand, fullPath, testCase, methodName);
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
