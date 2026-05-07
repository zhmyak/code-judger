using System.ComponentModel.DataAnnotations;

namespace OnlineJudger.API.Contracts
{
    public class AuthUserRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Имя должно быть размером от 5 до 50 символов")]
        public string Username { get; set; } = null!;
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Пароль должен быть размером от 5 до 50 символов")]
        public string Password { get; set; } = null!;
    }
}
