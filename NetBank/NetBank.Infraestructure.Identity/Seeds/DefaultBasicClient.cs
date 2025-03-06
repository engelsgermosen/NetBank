using Microsoft.AspNetCore.Identity;
using NetBank.Core.Application.Enums;
using NetBank.Infraestructure.Identity.Entities;

namespace NetBank.Infraestructure.Identity.Seeds
{
    public static class DefaultBasicClient
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {

            ApplicationUser defaultClient = new()
            {
                UserName="basicClient",
                Email= "basicClient@gmail.com",
                Name= "Michael",
                LastName= "Jordan",
                EmailConfirmed= true,
                PhoneNumberConfirmed= true,
                Identification = "402-13955839-4",
            };


            if(userManager.Users.All(u => u.Id != defaultClient.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultClient.Email);

                if(user == null)
                {
                    await userManager.CreateAsync(defaultClient, "123Enm#@word!");
                    await userManager.AddToRoleAsync(defaultClient, Roles.Client.ToString());
                }
            }


        }
    }
}
