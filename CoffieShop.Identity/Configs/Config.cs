using IdentityServer4;
using IdentityServer4.Models;

namespace CoffieShop.Identity.Configs
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> { "role" }
                },
                new IdentityResource("custom.country" , "current country",new[]{"country"})
            };

        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
                new ApiScope("ApiScopes.CoffeeAPI.read"),
                new ApiScope("ApiScopes.CoffeeAPI.write"),
                new ApiScope("ApiScopes.CoffeeAPI.fullAccess")
        };

        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
                new ApiResource("ApiResources.CoffeeAPI" , "ApiResources CoffeeAPI EndPoint" , new[]{"role" , "country"})
                {
                    Scopes =
                    {
                        "ApiScopes.CoffeeAPI.fullAccess"
                       ,"ApiScopes.CoffeeAPI.read"
                       ,"ApiScopes.CoffeeAPI.write"
                    },
                    ApiSecrets = { new Secret("c6Zyz7oDyZyZ22q3U0l8SWHMzCMF".Sha256())} // for working with Reference Token
                }
        };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "coffie.mvc",
                    ClientName = "Coffie MVC Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = { new Secret("c6Zyz7oDyZyZ22q3U0l8SWHMzCMF".Sha256()) },
                    AllowedScopes = { "openid", "profile", "ApiScopes.CoffeeAPI.read" },
                    RedirectUris = { "https://localhost:5444/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:5444/signout-callback-oidc" },
                    AllowOfflineAccess = true,

                },
                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "coffie.api",
                    ClientName = "Coffie Endpoints",
                    ClientSecrets = { new Secret("c6Zyz7oDyZyZ22q3U0l8SWHMzCMF".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "ApiScopes.CoffeeAPI.read", "ApiScopes.CoffeeAPI.write" }
                },
            };
    }
}
