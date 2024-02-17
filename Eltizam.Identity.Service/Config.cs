using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Eltizam.Identity.Service
{
    public static class Config
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
            new[]
            {
                new ApiResource("CoffeeAPI")
                {
                    Scopes =
                    {
                        "ApiScopes.CoffeeAPI.fullAccess"
                       ,"ApiScopes.CoffeeAPI.read"
                       ,"ApiScopes.CoffeeAPI.write"
                    },
                    ApiSecrets = { new Secret("c6Zyz7oDyZyZ22q3U0l8SWHMzCMF".Sha256())},
                    UserClaims = new List<string> { "role" , "country" }
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "coffie.Client",
                    ClientName = "Coffie Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    //AccessTokenType = AccessTokenType.Reference,
                    AccessTokenLifetime = 3600, // 1 hour
                    RedirectUris =
                    {
                        // This is Client Url
                        "https://localhost:5444/signin-oidc"
                    },
                     PostLogoutRedirectUris =
                    {
                        // This is Client Url
                        "https://localhost:5444/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile,
                       IdentityServerConstants.StandardScopes.Phone,
                       "role",
                       "custom.country",
                       "ApiScopes.CoffeeAPI.read"
                    },
                    ClientSecrets =
                    {
                        new Secret("c6Zyz7oDyZyZ22q3U0l8SWHMzCMF".Sha256())
                    }
                 }
            };
    }
}