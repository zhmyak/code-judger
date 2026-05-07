using System.ComponentModel.DataAnnotations;

namespace OnlineJudger.API.Contracts
{
    public class RegistrUserRequest
    {
        [Required(ErrorMessage = "Имя пользователя не должно быть пустым")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Имя должно быть размером от 5 до 50 символов")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Пароль не должен быть пустым")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Пароль должен быть размером от 5 до 50 символов")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Почта не должна быть пустой")]
        [EmailAddress(ErrorMessage = "Некорректный формат электронной почты")]
        [StringLength(150, ErrorMessage = "Длина почты не должна превышать 150 символов")]
        public string Email { get; set; } = null!;

    }
}
