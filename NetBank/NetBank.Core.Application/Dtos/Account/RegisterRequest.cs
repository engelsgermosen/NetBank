using NetBank.Core.Application.Enums;

namespace NetBank.Core.Application.Dtos.Account
{
    public class RegisterRequest
    {
        public string Id { get; set; }
        public  string Name { get; set; }
        public  string LastName { get; set; }
        public  string Email { get; set; }

        public  string UserName { get; set; }

        public  string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Identification { get; set; }

        public string? Phone {  get; set; }

        public decimal? InitialAmount { get; set; }

        public bool IsActive { get; set; } = true;

        public string RolId {  get; set; }
    }
}
