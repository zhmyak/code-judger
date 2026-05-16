using OnlineJudger.Domain.Entities;
using OnlineJudger.JudgeWorker.Interfaces;
using OnlineJudger.JudgeWorker.Models;
using System.Text.Json;

namespace OnlineJudger.JudgeWorker.Services
{
    public class TestRunner : ITestRunner
    {
        private ISandboxExecuter _sandboxExecuter;
        public TestRunner(ISandboxExecuter sandboxExecuter)
        {
            _sandboxExecuter = sandboxExecuter;
        }
        public async Task<TestRunResult> RunTestAsync(string runCommand, string path, TestCase testCase, string methodName)
        {

            var testData = new
            {
                harness_type = "standard",
                function_name = methodName,
                test_case = new
                {
                    inputs = JsonSerializer.Deserialize<JsonElement>(testCase.InputData),
                    expected = JsonSerializer.Deserialize<JsonElement>(testCase.ExpectedOutput)
                }
            };
            var testDataJson = JsonSerializer.Serialize(testData);
            var exec = await _sandboxExecuter.ExecuteAsync(new(runCommand, path, testDataJson));
            if (exec.IsTimeout)
            {
                return new TestRunResult
                {
                    Success = false,
                    Status = Domain.Enums.SubmissionStatus.TimeLimit,
                    ErrorMessage = "Превышено время выполнения кода"
                };
            }
            if (exec.IsMemoryOut)
            {
                return new TestRunResult
                {
                    Success = false,
                    Status = Domain.Enums.SubmissionStatus.MemoryLimit,
                    ErrorMessage = "Превышен размер выделенной памяти"
                };
            }
            if (exec.IsRuntimeError)
            {
                return new TestRunResult
                {
                    Success = false,
                    Status = Domain.Enums.SubmissionStatus.RuntimeError,
                    ErrorMessage = "Ошибка в ходе выполнении кода:\n" + exec.ErrorMessage
                };
            }

            var outputJson = JsonSerializer.Deserialize<JsonElement>(exec.Output);
            var expectedOutputJson = JsonSerializer.Deserialize<JsonElement>(testCase.ExpectedOutput);
            var actualOutput = outputJson.GetProperty("result").ToString();
            actualOutput = actualOutput.Replace(" ", "");
            var expectedOutput = expectedOutputJson.GetProperty("value").ToString();
            expectedOutput = expectedOutput.Replace(" ", "");
            bool passed = JsonElement.DeepEquals(outputJson.GetProperty("result"), expectedOutputJson.GetProperty("value"));
            if (!passed)
            {
                return new TestRunResult
                {
                    Success = false,
                    Status = Domain.Enums.SubmissionStatus.WrongAnswer,
                    ErrorMessage = $"Неверный ответ\nПри выходных данных: {testData.test_case.inputs}\nОжидаемый результат: {expectedOutput}\nФактический результат: {actualOutput}",
                    ActualOutput = actualOutput,
                    ExpectedOutput = expectedOutput
                };
            }
            return new TestRunResult
            {
                Success = true,
                Status = Domain.Enums.SubmissionStatus.Accepted,
                ActualOutput = actualOutput,
                ExpectedOutput = expectedOutput
            };
        }
    }
}
