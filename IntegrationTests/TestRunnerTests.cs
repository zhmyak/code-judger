using OnlineJudger.Domain.Entities;
using OnlineJudger.JudgeWorker.Services;

namespace IntegrationTests
{
    public class TestRunnerTests
    {
        [Fact]
        public async Task TestRunner_SuccessWithCorrectCode()
        {
            var fileExecuter = new FileExecuter();
            var testRunner = new TestRunner(fileExecuter);
            Submission submission = new()
            {
                Id = 1234,
                SourceCode = @"
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

        return None     ",
                Language = new()
                {
                    Name = "Python",
                    RunCommand = "python",
                    FileName = "main.py"
                }
            };
            TestCase testCase = new()
            {
                InputData = @"[
      { ""type"": ""array"", ""value"": [3, 3] },
      { ""type"": ""int"",   ""value"": 6 }
    ]",
                ExpectedOutput= @"{ ""type"": ""array"", ""value"": [0, 1]}",
            };
            string methodName = "twoSum";
            var testRunResult = await testRunner.RunTestAsync(submission, testCase, methodName);
            Directory.Delete("tmp", true);

            Assert.True(testRunResult.Success);
        }
    }
}
