using OnlineJudger.Domain.Entities;
using OnlineJudger.JudgeWorker.Interfaces;
using OnlineJudger.JudgeWorker.Models;
using System.Diagnostics;

namespace OnlineJudger.JudgeWorker.Services
{
    public class CompilerService : ICompiler
    {
        public async Task<CompilationResult> CompileAsync(Language language, string workDir)
        {
            if (language.Name == "C#")
            {
                File.WriteAllText(Path.Combine(workDir, "Solution.csproj"), @"
<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
</Project>");

                var processInfo = new ProcessStartInfo
                {
                    FileName = language.CompileCommand,
                    Arguments = $"build {workDir} -o {workDir}/bin",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                using (var build = Process.Start(processInfo))
                {
                    if (build == null)
                    {
                        throw new Exception("Не удалось запустить процесс");
                    }
                    string output = await build.StandardOutput.ReadToEndAsync();
                    string errors = await build.StandardError.ReadToEndAsync();
                    await build.WaitForExitAsync();
                    if (build.ExitCode != 0 || !string.IsNullOrEmpty(errors))
                    {
                        Console.WriteLine(errors);
                        return CompilationResult.Failure(errors);
                    }

                    return CompilationResult.Success(Path.Combine(workDir, "bin/Solution.dll"));

                }
            }
            else if (language.Name == "Java")
            {
                string jarPath = @"tmp\lib\json-20240303.jar";
                var processInfo = new ProcessStartInfo
                {
                    FileName = language.CompileCommand,
                    Arguments = $"-cp .;{jarPath} {Path.Combine(workDir, language.FileName)}",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                using (var build = Process.Start(processInfo))
                {
                    if (build == null)
                    {
                        throw new Exception("Не удалось запустить процесс");
                    }
                    string output = await build.StandardOutput.ReadToEndAsync();
                    string errors = await build.StandardError.ReadToEndAsync();
                    await build.WaitForExitAsync();
                    if (build.ExitCode != 0 || !string.IsNullOrEmpty(errors))
                    {
                        Console.WriteLine(errors);
                        return CompilationResult.Failure(errors);
                    }

                    return CompilationResult.Success(workDir);

                }
            }
            else
            {
                throw new NotImplementedException($"Компиляция для языка {language.Name} не реализована");
            }
        }
    }
}
