using Microsoft.Extensions.DependencyInjection;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.Services;
using System.Reflection;

namespace NetBank.Core.Application
{
    public static class ServiceDI
    {
        public static void AddApplicationLayer(this IServiceCollection service)
        {
            service.AddAutoMapper(Assembly.GetExecutingAssembly());
            service.AddTransient<IUserService,UserService>();
            service.AddTransient<IProductService,ProductService>();
            service.AddTransient<IBeneficiareService,BeneficiareService>();
            service.AddTransient<ITransactionService,TransactionService>();
        }
    }
}
