namespace BattleShip.WEB.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Некорректный адресс электронной почты")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Логин")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Длина логина должна быть от 2 до 20 символов")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        //[Required]
        //[Compare("Password", ErrorMessage = "Пароли не совпадают")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Подтвердить пароль")]
        //public string PasswordConfirm { get; set; }
    }
}
