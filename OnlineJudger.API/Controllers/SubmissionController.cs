using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineJudger.API.Contracts;
using OnlineJudger.Application.Services;
using System.Security.Claims;


namespace OnlineJudger.API.Controllers
{
    [ApiController]
    [Route("api/submissions")]
    [Authorize]
    public class SubmissionController : ControllerBase
    {
        private readonly SubmissionService _submissionService;
        private readonly ILogger<SubmissionController> _logger;

        public SubmissionController(SubmissionService submissionService, ILogger<SubmissionController> logger)
        {
            _submissionService = submissionService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddSubmission(CreateSubmissionRequest request)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            int createdSubmissionId = await _submissionService.AddAsync(userId, request.ProblemId, request.LanguageId, request.SourceCode);
            _logger.LogInformation($"Новый submission в {DateTimeOffset.Now}");
            return Ok(new
            {
                Id = createdSubmissionId
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> CheckSubmissionStatus(int id)
        {
            var submissionInfo = await _submissionService.GetSubmissionInfo(id);
            return Ok(submissionInfo);
        }
    }
}
