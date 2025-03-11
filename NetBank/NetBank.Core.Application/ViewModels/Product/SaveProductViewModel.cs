using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Application.ViewModels.Product
{
    public class SaveProductViewModel
    {
        public int AccountNumber { get; set; }
        public decimal? Balance { get; set; }

        public decimal? CreditLimit { get; set; }

        public decimal? AmountOwed { get; set; }

        public ProductType ProductType { get; set; }

        public string UserId { get; set; }

        public bool IsMain { get; set; } = false;
    }
}
