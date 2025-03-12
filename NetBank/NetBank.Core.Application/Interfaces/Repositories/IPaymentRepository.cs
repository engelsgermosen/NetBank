using NetBank.Core.Application.Services.Repositories;
using NetBank.Core.Domain.Entities;

namespace NetBank.Core.Application.Interfaces.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
    }
}
