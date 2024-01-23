using IdentityModel;
using System.Security.Claims;
using Duende.IdentityServer.Test;

namespace Marvin.IDP;

public class TestUsers
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
                        new Claim("role", "Basic"),
                        new Claim("country", "USA"),
                        new Claim(JwtClaimTypes.GivenName, "David"),
                        new Claim(JwtClaimTypes.FamilyName, "Flagg")
                    }
                },
                new TestUser
                {
                    SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    Username = "Emma",
                    Password = "P@$$w0rd",
                    Claims = new List<Claim>
                    {
                        new Claim("role", "Admin"),
                        new Claim("country", "BR"),
                        new Claim(JwtClaimTypes.GivenName, "Emma"),
                        new Claim(JwtClaimTypes.FamilyName, "Flagg")
                    }
                },
                new TestUser
                {
                    SubjectId = "1086ad19-81e6-441e-bad2-08dc11ba36d4",
                    Username = "M2ri",
                    Password = "P@$$w0rd",
                    Claims = new List<Claim>
                    {
                        new Claim("role", "Admin"),
                        new Claim("country", "EG"),
                        new Claim(JwtClaimTypes.GivenName, "Mahmoud"),
                        new Claim(JwtClaimTypes.FamilyName, "El Torri")
                    }
                }
            };
        }
    }
}