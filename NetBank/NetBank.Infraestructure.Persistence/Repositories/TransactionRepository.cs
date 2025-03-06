using NetBank.Core.Application.Services.Repositories;
using NetBank.Core.Domain.Entities;
using NetBank.Infraestructure.Persistence.Context;

namespace NetBank.Infraestructure.Persistence.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext context;

        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
