using NetBank.Core.Application.Services.Repositories;
using NetBank.Core.Domain.Entities;
using NetBank.Infraestructure.Persistence.Context;

namespace NetBank.Infraestructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product> , IProductRepository
    {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
