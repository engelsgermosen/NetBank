using Microsoft.AspNetCore.Identity;
using NetBank.Core.Application.Enums;
using NetBank.Infraestructure.Identity.Entities;

namespace NetBank.Infraestructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {

            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Client.ToString()));

        }
    }
}
