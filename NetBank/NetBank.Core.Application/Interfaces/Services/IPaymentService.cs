using NetBank.Core.Application.ViewModels.Payment;
using NetBank.Core.Domain.Entities;

namespace NetBank.Core.Application.Interfaces.Services
{
    public interface IPaymentService : IGenericService<SavePaymentViewModel,PaymentViewModel, Payment>
    {

        Task<PaymentCountViewModel> GetPaymentCount();
        Task<SavePaymentViewModel> PaymentExpressAndBeneficiarie(SavePaymentViewModel payment);


        Task<SavePaymentViewModel> ConfirmPaymentExpressAndBeneficiarie(SavePaymentViewModel payment);

        Task<SavePaymentViewModel> PaymentCreditCard(SavePaymentViewModel payment);

        Task<SavePaymentViewModel> PaymentLoan(SavePaymentViewModel payment);
    }
}
