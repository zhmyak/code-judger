using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineJudger.Domain.Entities;
using OnlineJudger.Domain.Enums;
using OnlineJudger.Infrastructure.Persistance;
namespace IntegrationTests
{
    public class SqlServerDbTests
    {
        private readonly string _connectionString;
        public SqlServerDbTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            _connectionString = config.GetConnectionString("SqlServer") ?? throw new Exception();
        }
        [Fact]
        public async Task DbContext_AddSubmissions()
        {
            var options = new DbContextOptionsBuilder<OnlineJudgeContext>().UseSqlServer(_connectionString).Options;
            var context = new OnlineJudgeContext(options);
            if (context.Submissions.FirstOrDefault() != null)
            {
                return;
            }
            var submissionsInitList = new List<Submission>
            { 
                //Accepted
                new Submission
                {
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
            foreach(var submission in submissionsInitList)
            {
                await context.AddAsync(submission);

            }
            await context.SaveChangesAsync();
            
            var submissions = await context.Submissions.ToListAsync();
            Assert.Equal(3, submissions.Count);

        }
        [Fact]
        public async Task DbContext_AddTestCases()
        {
            var options = new DbContextOptionsBuilder<OnlineJudgeContext>().UseSqlServer(_connectionString).Options;
            var context = new OnlineJudgeContext(options);
            if (context.TestCases.FirstOrDefault() != null)
            {
                return;
            }
            var testCasesInitList = new List<TestCase>
            {
                new TestCase
                {
                    ProblemId = 1,
                    InputData=@"[{ ""type"": ""array"", ""value"": [3, 3] },{ ""type"": ""int"",   ""value"": 6 }]",
                    ExpectedOutput=@"{""type"": ""array"", ""value"": [0, 1]}"
                },
                new TestCase
                {
                    ProblemId = 1,
                    InputData=@"[{ ""type"": ""array"", ""value"": [3, 2, 4] },{ ""type"": ""int"",   ""value"": 6 }]",
                    ExpectedOutput=@"{""type"": ""array"", ""value"": [1, 2]}"
                },
                new TestCase
                {
                    ProblemId = 1,
                    InputData=@"[{ ""type"": ""array"", ""value"": [2, 7, 11, 15] },{ ""type"": ""int"",   ""value"": 9 }]",
                    ExpectedOutput=@"{""type"": ""array"", ""value"": [0, 1]}"
                }
            };
            foreach (var testCase in testCasesInitList)
            {
                await context.AddAsync(testCase);
            }
            await context.SaveChangesAsync();
            var testCases = await context.TestCases.ToListAsync();
            Assert.Equal(3, testCases.Count);
        }
    }
}
