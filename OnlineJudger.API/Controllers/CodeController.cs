using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineJudger.Application.Services;
using System.Security.Claims;

namespace OnlineJudger.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/code")]
    public class CodeController : ControllerBase
    {
        private readonly CodeService _codeService;

        public CodeController(CodeService codeService)
        {
            _codeService = codeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetSavedCodeOrTemplate(int problemId, int languageId)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            string codeTemplate = await _codeService.GetSavedCodeOrTemplate(problemId, languageId, userId);
            return Ok(new
            {
                code = codeTemplate,
            });
        }
    }
}
