using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OnlineJudger.API.Contracts;
using OnlineJudger.Application.Services;

namespace OnlineJudger.API.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("api/login")]
        public async Task<IActionResult> LoginUser(AuthUserRequest request)
        {
            string accessToken = await _userService.AuthUser(request.Username, request.Password);
            _logger.LogInformation($"Аутентифицирован юзер {request.Username}");
            return Ok(new
            {
                token = accessToken,
            });
        }
        [HttpPost("api/registration")]
        public async Task<IActionResult> RegistrUser(RegistrUserRequest request)
        {
            string accessToken = await _userService.RegistrUser(request.Username, request.Password, request.Email);
            _logger.LogInformation($"Зарегистрирован новый юзер {request.Username}");
            return Ok(new
            {
                token = accessToken,
            });
        }
        [Authorize]
        [HttpGet("api/me")]
        public async Task<IActionResult> Me()
        {
            var claimsList = User.Claims.Select(claim => new { claim.Type, claim.Value }).ToList();
            return Ok(claimsList);
        }
    }
}
