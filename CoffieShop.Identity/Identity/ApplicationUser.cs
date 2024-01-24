using Microsoft.AspNetCore.Identity;

namespace CoffieShop.Identity.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }
}
