using Microsoft.AspNetCore.Mvc;
using OnlineJudger.Domain.Stores;
using OnlineJudger.API.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace OnlineJudger.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/languages")]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ILogger<LanguageController> _logger;
        public LanguageController(ILanguageRepository languageRepository, ILogger<LanguageController> logger)
        {
            _languageRepository = languageRepository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLanguages()
        {
            var languages = await _languageRepository.GetAllAsync();
            var result = languages.Select(l => new LanguageResponse()
            {
                Id = l.Id,
                Name = l.Name,
            });
            _logger.LogInformation("Получен список ЯП");
            return Ok(result);
        }
       
    }
}
