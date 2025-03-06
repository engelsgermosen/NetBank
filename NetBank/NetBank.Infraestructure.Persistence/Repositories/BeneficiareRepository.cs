

using NetBank.Core.Application.Services.Repositories;
using NetBank.Core.Domain.Entities;
using NetBank.Infraestructure.Persistence.Context;

namespace NetBank.Infraestructure.Persistence.Repositories
{
    public class BeneficiareRepository : GenericRepository<Beneficiarie>, IBeneficiareRepository
    {
        private readonly ApplicationDbContext context;

        public BeneficiareRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
