using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public decimal Amonut { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public PaymentType PaymentType { get; set; }
        public Product? OriginProduct { get; set; }

        public int? OriginAccountNumber { get; set; }

        public Product? DestinationProduct { get; set; }

        public int? DestinationAccountNumber { get; set; }
    }
}
