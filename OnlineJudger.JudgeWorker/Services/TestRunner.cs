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
        public async Task<TestRunResult> RunTestAsync(Submission submission, TestCase testCase, string methodName)
        {
            var language = submission.Language;
            string sourceCode = submission.SourceCode;

            var workDir = Path.Combine("tmp/submissions", submission.Id.ToString());
            Directory.CreateDirectory(workDir);
            string fileName = language.FileName;
            string fullPath = Path.Combine(workDir, fileName);

            /* switch (language.Name)
             {
                 case "Python":
                     fullCode = sourceCode + Harness.GetPythonHarness();
                     break;
                 case "cpp":
                     throw new Exception("cpp is not implemented yet");
                 default:
                     throw new Exception($"uknown language: {language.Name}");
             }*/
            string harness = language.Name switch
            {
                "Python" => Harness.GetPythonHarness(),
                "cpp" => throw new Exception("cpp not implemented yet"),
                _ => throw new Exception($"{language.Name} not implemented yet")
            };

            string fullCode = sourceCode + harness;
            await File.WriteAllTextAsync(fullPath, fullCode);

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
            var exec = await _sandboxExecuter.ExecuteAsync(new(language.RunCommand, fullPath, testDataJson));
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
            var expectedOutput = expectedOutputJson.GetProperty("value").ToString();
            if(actualOutput != expectedOutput)
            {
                return new TestRunResult
                {
                    Success = false,
                    Status = Domain.Enums.SubmissionStatus.WrongAnswer,
                    ErrorMessage = "Неверный ответ",
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
