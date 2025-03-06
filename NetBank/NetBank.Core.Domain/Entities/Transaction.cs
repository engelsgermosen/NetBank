using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public required DateTime TransactionDate { get; set; } = DateTime.Now;

        public TransactionType TransactionType { get; set; }
        public required bool State {  get; set; }

        public Product? OriginProduct { get; set; }

        public int? OriginProductId { get; set; }


        public Product? DestinationProduct { get; set; }

        public int? DestinationProductId { get; set; }

    }
}
