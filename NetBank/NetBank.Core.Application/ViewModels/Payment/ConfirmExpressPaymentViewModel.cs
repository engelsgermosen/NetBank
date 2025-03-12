using NetBank.Core.Application.Dtos.Account;

namespace NetBank.Core.Application.ViewModels.Payment
{
    public class ConfirmExpressPaymentViewModel
    {
        public AuthenticationResponse Usuario { get; set; }
        public SavePaymentViewModel PagoConfirmacion { get; set; }
    }
}
