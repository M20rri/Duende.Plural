using Microsoft.AspNetCore.Identity;

namespace Eltizam.Identity.Service.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string LastName { get; set; }
    }
}
