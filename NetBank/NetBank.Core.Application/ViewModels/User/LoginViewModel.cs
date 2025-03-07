using System.ComponentModel.DataAnnotations;

namespace NetBank.Core.Application.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="El nombre de usuario es obligatorio")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatorio")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool HasError { get; set; }

        public string? Error {  get; set; }
    }
}
