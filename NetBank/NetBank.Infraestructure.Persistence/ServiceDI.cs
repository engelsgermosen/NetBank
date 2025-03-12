using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBank.Core.Application.Interfaces.Repositories;
using NetBank.Core.Application.Services.Repositories;
using NetBank.Infraestructure.Persistence.Context;
using NetBank.Infraestructure.Persistence.Repositories;

namespace NetBank.Infraestructure.Persistence
{
    public static class ServiceDI
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                m => m.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

            services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IBeneficiareRepository, BeneficiareRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
        }
    }
}
