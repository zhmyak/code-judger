using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineJudger.API.Contracts;
using OnlineJudger.Application.Services;
using System.Security.Claims;

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
                username = request.Username
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
                username = request.Username
            });
        }
        [Authorize]
        [HttpGet("api/me")]
        public async Task<IActionResult> Me()
        {
            var claimsList = User.Claims.Select(claim => new { claim.Type, claim.Value }).ToList();
            return Ok(claimsList);
        }

        [Authorize]
        [HttpGet("api/users/{id}")]
        public async Task<IActionResult> GetUserInfo(int id)
        {
            var userInfo = await _userService.GetUserInfo(id);
            return Ok(userInfo);
        }
        [Authorize]
        [HttpGet("api/user")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userInfo = await _userService.GetUserInfo(userId);
            return Ok(userInfo);
        }
        [Authorize]
        [HttpGet("api/toplist")]
        public async Task<IActionResult> GetTopList()
        {
            var topList = await _userService.GetTopList();
            return Ok(topList);
        }
    }
}
