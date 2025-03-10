using NetBank.Core.Application.ViewModels.Transaction;
using NetBank.Core.Domain.Entities;

namespace NetBank.Core.Application.Interfaces.Services
{
    public interface ITransactionService : IGenericService<SaveTransactionViewModel, TransactionViewModel, Transaction>
    {
        Task<TransaccionCountViewModel> GetTransaccionCount();
    }
}
