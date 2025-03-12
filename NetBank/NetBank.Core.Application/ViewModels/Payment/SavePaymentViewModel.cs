using NetBank.Core.Application.ViewModels.Product;
using NetBank.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace NetBank.Core.Application.ViewModels.Payment
{
    public class SavePaymentViewModel
    {
        [Required(ErrorMessage ="El monto es obligatorio")]
        public decimal Amonut { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public PaymentType PaymentType { get; set; }

        [Required(ErrorMessage ="El numero de destino es obligatorio")]
        [Range(780000000,int.MaxValue,ErrorMessage ="El numero de cuenta no es valido")]
        public int OriginAccountNumber { get; set; }

        [Required(ErrorMessage ="El numero de cuenta de destino es obligatorio")]
        [RegularExpression("^780\\d{6}$", ErrorMessage = "El formato debe ser 780xxxxxx, donde x son números.")]
        public int DestinationAccountNumber { get; set; }

        public ICollection<ProductViewModel>? Accounts { get; set; }

        public bool HasError = false;

        public string? Error {  get; set; }

        public string? UserId { get; set; }
    }
}
