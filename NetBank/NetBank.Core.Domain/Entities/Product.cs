using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public decimal Balance { get; set; }

        public decimal? CreditLimit { get; set; }

        public ProductType ProductType { get; set; }

        public required bool IsMain { get; set; } = false;

        public User? User { get; set; }

        public int? UserId { get; set; }

        public ICollection<Transaction>? OriginTransactions { get; set; }

        public ICollection<Transaction>? DestinationTransactions { get; set; }
    }
}
