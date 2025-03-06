using NetBank.Core.Application.Enums;

namespace NetBank.Core.Application.Dtos.Account
{
    public class RegisterRequest
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }

        public required string UserName { get; set; }

        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
        public required string Identification { get; set; }

        public required string Phone {  get; set; }

        public decimal? InitialAmount { get; set; }

        public bool IsActive { get; set; } = true;

        public Roles Rol {  get; set; }
    }
}
