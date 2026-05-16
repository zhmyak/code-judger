using Microsoft.EntityFrameworkCore;
using OnlineJudger.Domain.Entities;
using OnlineJudger.Domain.Enums;
using OnlineJudger.Infrastructure.Persistance;

namespace IntegrationTests
{
    public class InMemoryDatabaseFixture : IAsyncLifetime
    {
        private DbContextOptions<OnlineJudgeContext> _options;
        public OnlineJudgeContext Context { get; private set; }

        public async Task DisposeAsync()
        {
            await Context.Database.EnsureDeletedAsync();
            Context?.Dispose();
        }

        public async Task InitializeAsync()
        {
            _options = new DbContextOptionsBuilder<OnlineJudgeContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            Context = new OnlineJudgeContext(_options);
            await Context.Database.EnsureCreatedAsync();
            await SeedDataAsync();
        }
        public async Task SeedDataAsync()
        {
            var problems = new List<Problem>
            {
                new Problem
                {
                    Id = 1,
                    Title = "Сумма двух",
                    Description = "",
                    Difficulty = Difficulty.Easy,
                    TimeLimitMs = 10000,
                    MemoryLimitMb = 100,
                    CreatedAt = DateTime.Now,
                    MethodName = "twoSum"
                }
            };
            var languages = new List<Language>
            {
                 new Language
                {
                    Id = 1,
                    Name = "Python",
                    FileName = "main.py",
                    RunCommand = "python",
                    DockerImage = "mock"
                }
            };
            var submissions = new List<Submission>
            { 
                //Accepted
                new Submission
                {
                    Id = 1,
                    UserId=1,
                    ProblemId=1,
                    LanguageId=1,
                    SourceCode=@"
class Solution(object):
    def twoSum(self, nums, target):
        """"""
        :type nums: List[int]
        :type target: int
        :rtype: List[int]
        """"""
        num_map = {}  # число -> индекс
    
        for i, num in enumerate(nums):
            complement = target - num
            
            if complement in num_map:
                return [num_map[complement], i]
        
            num_map[num] = i

        return None",
                    Status = SubmissionStatus.Pending,
                    CreatedAt=DateTime.Now,
                    UpdatedAt=DateTime.Now
                },
                 new Submission
                {
                     //Wrong Answer
                    Id = 2,
                    UserId=1,
                    ProblemId=1,
                    LanguageId=1,
                    SourceCode=@"
class Solution(object):
    def twoSum(self, nums, target):
        """"""
        :type nums: List[int]
        :type target: int
        :rtype: List[int]
        """"""
        num_map = {}  # число -> индекс
    
        for i, num in enumerate(nums):
            complement = target - num
            
            if complement in num_map:
                return [num_map[complement], i + 1]
        
            num_map[num] = i

        return None",
                    Status = SubmissionStatus.Pending,
                    CreatedAt=DateTime.Now,
                    UpdatedAt=DateTime.Now
                },
                 //Runtime Error
                new Submission
                {
                    Id = 3,
                    UserId=1,
                    ProblemId=1,
                    LanguageId=1,
                    SourceCode=@"
class Solution(object):
    def twoSum(self, nums, target):
        """"""
        :type nums: List[int]
        :type target: int
        :rtype: List[int]
        """"""
        num_map = {}  # число -> индекс
    
        for i, num in enumerate(nums):
            complement = target - num
            
            if complement in num_map:
                return [num_map[complement], i]ashdbashdbhb
        
            num_map[num] = i

        return None",
                    Status = SubmissionStatus.Pending,
                    CreatedAt=DateTime.Now,
                    UpdatedAt=DateTime.Now
                }
            };
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    Id = 1,
                    ProblemId = 1,
                    InputData=@"[{ ""type"": ""array"", ""value"": [3, 3] },{ ""type"": ""int"",   ""value"": 6 }]",
                    ExpectedOutput=@"{""type"": ""array"", ""value"": [0, 1]}"
                },
                new TestCase
                {
                    Id = 2,
                    ProblemId = 1,
                    InputData=@"[{ ""type"": ""array"", ""value"": [3, 2, 4] },{ ""type"": ""int"",   ""value"": 6 }]",
                    ExpectedOutput=@"{""type"": ""array"", ""value"": [1, 2]}"
                },
                new TestCase
                {
                    Id = 3,
                    ProblemId = 1,
                    InputData=@"[{ ""type"": ""array"", ""value"": [2, 7, 11, 15] },{ ""type"": ""int"",   ""value"": 9 }]",
                    ExpectedOutput=@"{""type"": ""array"", ""value"": [0, 1]}"
                }

            };
            foreach (var language in languages)
            {
                Context.Languages.Add(language);
            }
            foreach (var problem in problems)
            {
                Context.Problems.Add(problem);
            }
            foreach (var submission in submissions)
            {
                Context.Submissions.Add(submission);
            }
            foreach (var testCase in testCases)
            {
                Context.TestCases.Add(testCase);
            }
            await Context.SaveChangesAsync();
        }
    }
}
