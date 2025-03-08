namespace NetBank.Core.Domain.Entities
{
    public class Beneficiarie
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string LastName { get; set; }

        public required string AccountNumber { get; set; }

        public string? UserId { get; set; }
    }
}
