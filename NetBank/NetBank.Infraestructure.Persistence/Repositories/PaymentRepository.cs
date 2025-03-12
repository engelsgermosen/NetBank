using NetBank.Core.Application.Interfaces.Repositories;
using NetBank.Core.Domain.Entities;
using NetBank.Infraestructure.Persistence.Context;

namespace NetBank.Infraestructure.Persistence.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext context;

        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
