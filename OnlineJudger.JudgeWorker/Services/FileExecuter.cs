using OnlineJudger.JudgeWorker.Interfaces;
using OnlineJudger.JudgeWorker.Models;
using System.Diagnostics;
namespace OnlineJudger.JudgeWorker.Services
{
    public class FileExecuter : ISandboxExecuter
    {
        public async Task<ExecutionResult> ExecuteAsync(ExecutionOptions options)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = options.RunCommand,
                Arguments = options.Path,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            if (options.RunCommand == "java")
            {
                string jarPath = @"tmp\lib\json-20240303.jar";
                processInfo = new ProcessStartInfo
                {
                    FileName = options.RunCommand,
                    Arguments = $"-cp .;{jarPath};{options.Path} Harness",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
            }
            using (var process = Process.Start(processInfo))
            {
                if (process == null)
                    throw new Exception("Не удалось запустить процесс");
                await process.StandardInput.WriteAsync(options.TestCaseJson);
                process.StandardInput.Close();
                string output = await process.StandardOutput.ReadToEndAsync();
                string errors = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();
                if (!string.IsNullOrEmpty(errors))
                {
                    Console.WriteLine(errors);
                    return ExecutionResult.RuntimeError(errors);
                }

                return new ExecutionResult
                {
                    Success = true,
                    Output = output
                };
            }
        }
    }
}
