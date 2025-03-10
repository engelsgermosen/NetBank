using Microsoft.AspNetCore.Identity;

namespace NetBank.Infraestructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public required string Name { get; set; }

        public required string LastName { get; set; }

        public required string Identification { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
