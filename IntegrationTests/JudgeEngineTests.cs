using OnlineJudger.Domain.Stores;
using OnlineJudger.Infrastructure.Persistance;
using OnlineJudger.JudgeWorker;
using OnlineJudger.JudgeWorker.Interfaces;
using OnlineJudger.JudgeWorker.Services;
using OnlineJudger.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class JudgeEngineTests : IClassFixture<SqlServerDatabaseFixture>
    {
        private readonly SqlServerDatabaseFixture _fixture;
        
        public JudgeEngineTests(SqlServerDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ProccessAsync_TwoSumUpdateStatusToAccepted()
        {

            ISandboxExecuter sandboxExecuter = new FileExecuter();
            ITestRunner testRunner = new TestRunner(sandboxExecuter);
            IProblemRepository problemRepository = new ProblemRepository(_fixture.Context);
            ITestCaseRepository testCaseRepository = new TestCaseRepository(_fixture.Context);
            ISubmissionRepository submissionRepository= new SubmissionRepository(_fixture.Context);
            IUnitOfWork unitOfWork = _fixture.Context;
            JudgeEngine judgeEngine = new JudgeEngine(
                testRunner,
                submissionRepository,
                testCaseRepository,
                problemRepository,
                unitOfWork
                );
            await judgeEngine.ProcessAsync(2);
            var submission = _fixture.Context.Submissions.FirstOrDefault(s => s.Id == 2);
            Assert.NotNull(submission);
            Assert.Null(submission.ErrorMessage);
            Assert.Equal(SubmissionStatus.Accepted, submission.Status);
        }
        [Fact]
        public async Task ProccessAsync_TwoSumUpdateStatusToWrongAnswer()
        {

            ISandboxExecuter sandboxExecuter = new FileExecuter();
            ITestRunner testRunner = new TestRunner(sandboxExecuter);
            IProblemRepository problemRepository = new ProblemRepository(_fixture.Context);
            ITestCaseRepository testCaseRepository = new TestCaseRepository(_fixture.Context);
            ISubmissionRepository submissionRepository = new SubmissionRepository(_fixture.Context);
            IUnitOfWork unitOfWork = _fixture.Context;
            JudgeEngine judgeEngine = new JudgeEngine(
                testRunner,
                submissionRepository,
                testCaseRepository,
                problemRepository,
                unitOfWork
                );
            await judgeEngine.ProcessAsync(3);
            var submission = _fixture.Context.Submissions.FirstOrDefault(s => s.Id == 3);
            Assert.NotNull(submission);
            Assert.NotNull(submission.ErrorMessage);
            Assert.Equal("Неверный ответ", submission.ErrorMessage);
            Assert.Equal(SubmissionStatus.WrongAnswer, submission.Status);
        }
        [Fact]
        public async Task ProccessAsync_TwoSumUpdateStatusToRuntimeError()
        {

            ISandboxExecuter sandboxExecuter = new FileExecuter();
            ITestRunner testRunner = new TestRunner(sandboxExecuter);
            IProblemRepository problemRepository = new ProblemRepository(_fixture.Context);
            ITestCaseRepository testCaseRepository = new TestCaseRepository(_fixture.Context);
            ISubmissionRepository submissionRepository = new SubmissionRepository(_fixture.Context);
            IUnitOfWork unitOfWork = _fixture.Context;
            JudgeEngine judgeEngine = new JudgeEngine(
                testRunner,
                submissionRepository,
                testCaseRepository,
                problemRepository,
                unitOfWork
                );
            await judgeEngine.ProcessAsync(4);
            var submission = _fixture.Context.Submissions.FirstOrDefault(s => s.Id == 4);
            Assert.NotNull(submission);
            Assert.NotNull(submission.ErrorMessage);
            Assert.Equal(SubmissionStatus.RuntimeError, submission.Status);
        }
        [Fact]
        public async Task ProccessAsync_IsPalindromeUpdateStatusToWrongAnswer()
        {

            ISandboxExecuter sandboxExecuter = new FileExecuter();
            ITestRunner testRunner = new TestRunner(sandboxExecuter);
            IProblemRepository problemRepository = new ProblemRepository(_fixture.Context);
            ITestCaseRepository testCaseRepository = new TestCaseRepository(_fixture.Context);
            ISubmissionRepository submissionRepository = new SubmissionRepository(_fixture.Context);
            IUnitOfWork unitOfWork = _fixture.Context;
            JudgeEngine judgeEngine = new JudgeEngine(
                testRunner,
                submissionRepository,
                testCaseRepository,
                problemRepository,
                unitOfWork
                );
            await judgeEngine.ProcessAsync(5);
            var submission = _fixture.Context.Submissions.FirstOrDefault(s => s.Id == 5);
            Assert.NotNull(submission);
            Assert.Null(submission.ErrorMessage);
            Assert.Equal(SubmissionStatus.Accepted, submission.Status);
        }

    }
}
