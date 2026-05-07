using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineJudger.API.Contracts;
using OnlineJudger.Application.Services;
using OnlineJudger.Domain.Stores;
using System.Security.Claims;

namespace OnlineJudger.API.Controllers
{
    [ApiController]
    [Route("api/problems")]
    [Authorize]
    public class ProblemController : ControllerBase
    {
        private readonly ILogger<ProblemController> _logger;
        private readonly ProblemService _problemService;
        public ProblemController(ILogger<ProblemController> logger, ProblemService problemService)
        {
            _logger = logger;
            _problemService = problemService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var problems = await _problemService.GetAllByUserIdAsync(userId);
            var result = problems.Select(p => new ProblemResponse()
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Difficulty = p.Difficulty.ToString(),
                IsSolved = p.IsSolved
            });
            _logger.LogInformation("Получен список задач");
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var problem = await _problemService.GetByIdAsync(id, userId);
            var problemDto = new ProblemResponse()
            {
                Id = problem.Id,
                Title = problem.Title,
                Description = problem.Description,
                Difficulty =  problem.Difficulty.ToString(),
                IsSolved = problem.IsSolved
            };
            _logger.LogInformation($"Получена задча по id: {id}");
            return Ok(problemDto);
        }
    }
}
