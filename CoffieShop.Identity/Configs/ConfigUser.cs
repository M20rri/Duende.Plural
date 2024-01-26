using IdentityModel;
using IdentityServer4.Test;
using System.Security.Claims;

namespace CoffieShop.Identity.Configs
{
    public class ConfigUser
    {
        public static List<TestUser> Users
        {
            get
            {
                return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    Username = "David",
                    Password = "P@$$w0rd",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Name, "David Flagg"),
                        new Claim(JwtClaimTypes.PreferredUserName, "Deav"),
                        new Claim(JwtClaimTypes.FamilyName, "Flagg"),
                        new Claim(JwtClaimTypes.GivenName, "David"),
                        new Claim(JwtClaimTypes.MiddleName, "Jeffy"),
                        new Claim(JwtClaimTypes.Gender, "Male"),
                        new Claim(JwtClaimTypes.Locale, "en"),
                        new Claim(JwtClaimTypes.PhoneNumber, "0115931334"),
                        new Claim(JwtClaimTypes.WebSite, "www.Deav.com"),
                        new Claim("role", "Basic"),
                        new Claim("country", "USA"),
                        new Claim("isMarried", "True"),
                        new Claim("hasChildren", "False"),
                        new Claim("religion", "Cristiam")
                    }
                },
                new TestUser
                {
                    SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    Username = "Emma",
                    Password = "P@$$w0rd",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Name, "Emma Flagg"),
                        new Claim(JwtClaimTypes.PreferredUserName, "Emy"),
                        new Claim(JwtClaimTypes.FamilyName, "Flagg"),
                        new Claim(JwtClaimTypes.GivenName, "Emma"),
                        new Claim(JwtClaimTypes.MiddleName, "Robert"),
                        new Claim(JwtClaimTypes.Gender, "Female"),
                        new Claim(JwtClaimTypes.Locale, "fr"),
                        new Claim(JwtClaimTypes.PhoneNumber, "01068011702"),
                        new Claim(JwtClaimTypes.WebSite, "www.Emy.com"),
                        new Claim("role", "Admin"),
                        new Claim("country", "BR"),
                        new Claim("isMarried", "False"),
                        new Claim("hasChildren", "True"),
                        new Claim("religion", "jewish")
                    }
                }
            };
            }
        }
    }
}
