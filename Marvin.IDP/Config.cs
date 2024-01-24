using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Marvin.IDP;

public static class Config
{

    /// <summary>
    ///  * Set of claims related to User
    ///  openid (required scope for OIDC) => Claims: sub(user identifier)
    ///  profile => Claims : (name, family_name, given_name, middle_name, nickname,  preferred_username, profile, picture, website, gender, birthdate, zoneinfo, locale, updated_at)
    ///  email => email, email_verified 
    ///  address => address
    ///  phone =>  phone_number, phone_number_verified
    ///  offline_access (used for long-lived access)
    /// </summary>
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Phone(),
            new IdentityResource("role" , "your role(s)",new[]{"role"}),
            new IdentityResource("custom.country" , "current country",new[]{"country"}),
            new IdentityResource(
            name: "custom.Personality",
            userClaims: new[] { "hasChildren" , "isMarried" , "religion"},
            displayName: "your family details")
        };


    /// <summary>
    /// Mapped to Api : client applications can access the apis
    /// these client applications are configured to clients (Read , Write , Update , Delete)
    /// appear in aud (audience) claim list
    /// </summary>
    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("Custom.MarvinApi.read"),
                new ApiScope("Custom.MarvinApi.write"),
                new ApiScope("Custom.MarvinApi.fullAccess")
            };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
            {
                new ApiResource("MarvinApi" , "Marvin EndPoint" , new[]{"role" , "country" , "phone_number" , "isMarried"})
                {
                    Scopes =
                    {
                        "Custom.MarvinApi.fullAccess"
                       ,"Custom.MarvinApi.read"
                       ,"Custom.MarvinApi.write"
                    },
                    ApiSecrets = { new Secret("apisecret".Sha256())} // for working with Reference Token
                }
            };


    /// <summary>
    /// Refers to Client Applications (Mobile , Web , Desktop , .. etc)
    /// </summary>
    public static IEnumerable<Client> Clients =>
        new Client[]
            {
                new Client
                {
                    ClientId = "Marvin-Client",
                    ClientName = "Marvin MVC Project",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowOfflineAccess = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AccessTokenType = AccessTokenType.Reference,
                    AccessTokenLifetime = 3600, // 1 hour
                    RedirectUris =
                    {
                        // This is Client Url
                        "https://localhost:7035/signin-oidc"
                    },
                     PostLogoutRedirectUris =
                    {
                        // This is Client Url
                        "https://localhost:7035/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile,
                       IdentityServerConstants.StandardScopes.Phone,
                       "role",
                       "custom.country",
                       "custom.Personality",
                       "Custom.MarvinApi.read"
                       //"Custom.MarvinApi.fullAccess" // for Endpoint Scope
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                 }
            };
}