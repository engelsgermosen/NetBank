using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Application.ViewModels.Payment
{
    public class SavePaymentViewModel
    {
        public decimal Amonut { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public PaymentType PaymentType { get; set; }

        public int? OriginAccountNumber { get; set; }

        public int? DestinationAccountNumber { get; set; }
    }
}
