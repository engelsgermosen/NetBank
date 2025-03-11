using Microsoft.AspNetCore.Identity;
using NetBank.Core.Application.Enums;
using NetBank.Infraestructure.Identity.Entities;

namespace NetBank.Infraestructure.Identity.Seeds
{
    public static class DefaultBasicAdmin
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {

            ApplicationUser defaultUser = new()
            {
                UserName="basicAdmin",
                Email= "basicAdmin@gmail.com",
                Name= "Lebron",
                LastName= "James",
                EmailConfirmed= true,
                PhoneNumberConfirmed= true,
                Identification = "402-13955839-9",
            };


            if(userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);

                if(user == null)
                {
                    await userManager.CreateAsync(defaultUser, "321Enm#@word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }
            }
        }
    }
}
