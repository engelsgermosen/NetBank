using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Infraestructure.Identity.Context;
using NetBank.Infraestructure.Identity.Entities;
using NetBank.Infraestructure.Identity.Seeds;
using NetBank.Infraestructure.Identity.Services;

namespace NetBank.Infraestructure.Identity
{
    public static class ServiceDI
    {
        public static void AddIdentityLayer(this IServiceCollection services, IConfiguration configuration)
        {

            #region Context

                services.AddDbContext<IdentityContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                    m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
                });

            #endregion

            #region Identity Config


            services.AddIdentityCore<ApplicationUser>()
               .AddRoles<IdentityRole>()
               .AddSignInManager()
               .AddEntityFrameworkStores<IdentityContext>()
               .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromSeconds(300);
            });


            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultSignInScheme = IdentityConstants.ApplicationScheme;

            }).AddCookie(IdentityConstants.ApplicationScheme, opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromHours(24);
                opt.LoginPath = "/User";
                opt.AccessDeniedPath = "/User/AccessDenied";
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User";
                options.AccessDeniedPath = "/User/AccesDenied";
            });

            #endregion

            #region DI
            services.AddTransient<IAccountService, AccountService>();
            #endregion
        }

        public static async  Task RunIdentitySeeds(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await DefaultRoles.SeedAsync(userManager, roleManager);
                    await DefaultBasicAdmin.SeedAsync(userManager, roleManager);
                    await DefaultBasicClient.SeedAsync(userManager, roleManager);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
