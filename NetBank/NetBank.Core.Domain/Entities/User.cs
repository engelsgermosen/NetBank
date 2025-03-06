using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string LastName { get; set; }
        public required string Identification { get; set; }

        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }

        public UserType UserType { get; set; }
        public required bool IsActive { get; set; }

        public decimal? InitialAmount { get; set; }

        //Navigation properties

        public ICollection<Beneficiarie>? Beneficiaries { get; set; }    

        public ICollection<Product>? Products { get; set; }
    }
}
