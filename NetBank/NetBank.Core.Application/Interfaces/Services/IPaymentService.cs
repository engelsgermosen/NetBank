using NetBank.Core.Application.ViewModels.Payment;
using NetBank.Core.Domain.Entities;

namespace NetBank.Core.Application.Interfaces.Services
{
    public interface IPaymentService : IGenericService<SavePaymentViewModel,PaymentViewModel, Payment>
    {

    }
}
