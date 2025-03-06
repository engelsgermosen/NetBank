using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Domain.Settings;
using NetBank.Infraestructure.Shared.Services;

namespace NetBank.Infraestructure.Shared
{
    public static class ServiceDI
    {
        public static void AddSharedLayer(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
