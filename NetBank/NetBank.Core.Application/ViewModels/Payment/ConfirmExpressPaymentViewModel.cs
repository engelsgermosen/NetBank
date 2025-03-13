using NetBank.Core.Application.Dtos.Account;

namespace NetBank.Core.Application.ViewModels.Payment
{
    public class ConfirmExpressAngBeneficiariePaymentViewModel
    {
        public AuthenticationResponse Usuario { get; set; }
        public SavePaymentViewModel PagoConfirmacion { get; set; }
    }
}
