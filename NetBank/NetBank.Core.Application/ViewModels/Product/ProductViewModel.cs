

using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Application.ViewModels.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public decimal Balance { get; set; }

        public decimal? CreditLimit { get; set; }

        public ProductType ProductType { get; set; }

        public required bool IsMain { get; set; } = false;

        public int? UserId { get; set; }
    }
}
