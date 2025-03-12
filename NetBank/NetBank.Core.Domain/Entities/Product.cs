using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Domain.Entities
{
    public class Product
    {

        public int AccountNumber { get; set; }
        public decimal? Balance { get; set; }

        public decimal? CreditLimit { get; set; }

        public decimal? AmountOwed { get; set; }

        public ProductType ProductType { get; set; }

        public bool IsMain { get; set; } = false;


        public string? UserId { get; set; }

        public ICollection<Transaction>? OriginTransactions { get; set; }

        public ICollection<Transaction>? DestinationTransactions { get; set; }

        public ICollection<Payment>? OriginPayments { get; set; }

        public ICollection<Payment>? DestinationPayments { get; set; }
    }
}
