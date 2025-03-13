using NetBank.Core.Application.ViewModels.Payment;
using NetBank.Core.Domain.Entities;

namespace NetBank.Core.Application.Interfaces.Services
{
    public interface IPaymentService : IGenericService<SavePaymentViewModel,PaymentViewModel, Payment>
    {
        Task<SavePaymentViewModel> PaymentExpressAndBeneficiarie(SavePaymentViewModel payment);

<<<<<<< HEAD
        Task<SavePaymentViewModel> ConfirmPaymentExpressAndBeneficiarie(SavePaymentViewModel payment);
=======
        Task<SavePaymentViewModel> PaymentCreditCard(SavePaymentViewModel payment);

        Task<SavePaymentViewModel> PaymentLoan(SavePaymentViewModel payment);

        Task<SavePaymentViewModel> ConfirmPaymentExpress(SavePaymentViewModel payment);
>>>>>>> natanael/app
    }
}
