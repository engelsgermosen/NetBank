using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBank.Core.Application.ViewModels.User
{
    public class UpdateUserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El correo electronico es obligatorio")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La cedula es obligatorio")]
        [DataType(DataType.Text)]

        public string Identification { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El confirmar contraseña es obligatorio")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "No coinciden las contraseñas")]
        public string ConfirmPassword { get; set; }


        [DataType(DataType.Currency)]
        public decimal? AditionalAmount { get; set; }


        public bool HasError { get; set; }
        public string? Error { get; set; }
        
        public bool IsAdmin { get; set; }
    }
}
