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
            new IdentityResource("idRoles" , "your role(s)",new[]{"role"}), // custom idRoles you can change name it's just alias
            new IdentityResource("idCountry" , "your country",new[]{"country"})
        };

    /// <summary>
    /// Mapped to Api : client applications can access the apis
    /// these client applications are configured to clients (Read , Write , Update , Delete)
    /// appear in aud (audience) claim list
    /// </summary>
    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("MarvinApi.fullAccess")
            };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
            {
                new ApiResource("MarvinApi" , "Marvin EndPoint" , new[]{"role"})
                {
                    Scopes = { "MarvinApi.fullAccess" }
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
                       "idRoles",
                       "idCountry",
                       "MarvinApi.fullAccess" // for Endpoint Scope
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                 }
            };
}