using System.ComponentModel.DataAnnotations;

namespace NetBank.Core.Application.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="El nombre de usuario es obligatorio")]
        [DataType(DataType.Text)]
        public required string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatorio")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
