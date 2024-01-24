using Marvin.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAccessTokenManagement();

builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7184/");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
}).AddUserAccessTokenHandler();

builder.Services.AddHttpClient("IDPClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.AccessDeniedPath = "/Authentication/AccessDenied";
})
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.Authority = "https://localhost:5001/";
    options.ClientId = "Marvin-Client";
    options.ClientSecret = "secret";
    options.ResponseType = "code";
    //options.Scope.Add("openid");
    //options.Scope.Add("profile");
    //options.CallbackPath = new PathString("signin-oidc");
    // SignedOutCallbackPath: default = host:port/signout-callback-oidc.
    // Must match with the post logout redirect URI at IDP client config if
    // you want to automatically return to the application after logging out
    // of IdentityServer.
    // To change, set SignedOutCallbackPath
    // eg: options.SignedOutCallbackPath = new PathString("pathaftersignout");

    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;

    // for remove un-needed claims

    options.ClaimActions.DeleteClaim("nonce");
    options.ClaimActions.DeleteClaim("aud");
    options.ClaimActions.DeleteClaim("azp");
    options.ClaimActions.DeleteClaim("acr");
    options.ClaimActions.DeleteClaim("amr");
    options.ClaimActions.DeleteClaim("iss");
    options.ClaimActions.DeleteClaim("iat");
    options.ClaimActions.DeleteClaim("nbf");
    options.ClaimActions.DeleteClaim("exp");
    options.ClaimActions.DeleteClaim("idp");
    options.ClaimActions.DeleteClaim("at_hash");
    options.ClaimActions.DeleteClaim("c_hash");
    options.ClaimActions.DeleteClaim("auth_time");
    options.ClaimActions.DeleteClaim("ipaddr");
    options.ClaimActions.DeleteClaim("platf");
    options.ClaimActions.DeleteClaim("ver");
    options.ClaimActions.DeleteClaim("amr");

    // Add ultra scope all existing in client AllowedScopes
    options.Scope.Add("Custom.MarvinApi.read");
    options.Scope.Add("offline_access");
    options.Scope.Add("role");
    options.Scope.Add("profile");
    options.Scope.Add("phone");
    options.Scope.Add("custom.Personality");
    options.Scope.Add("custom.country");

    #region MapJsonKey

    /// <summary>
    /// options.ClaimActions.MapJsonKey applied only for UserClaims .
    /// ex : new IdentityResource("custom.country" , "current country",new[]{"country"})
    /// we will appy it on the UserClaims is "country"
    /// </summary>

    #region included in custom.country 
    options.ClaimActions.MapJsonKey("country", "country");
    #endregion

    #region included in profile
    options.ClaimActions.MapJsonKey("gender", "gender");
    options.ClaimActions.MapJsonKey("locale", "locale");
    options.ClaimActions.MapJsonKey("preferred_username", "preferred_username");
    options.ClaimActions.MapJsonKey("website", "website");
    #endregion

    #region included in phone
    options.ClaimActions.MapJsonKey("phone_number", "phone_number");
    #endregion

    #region included in role
    options.ClaimActions.MapJsonKey("role", "role");
    #endregion

    #region included in custom.Personality
    options.ClaimActions.MapJsonKey("hasChildren", "hasChildren");
    options.ClaimActions.MapJsonKey("isMarried", "isMarried");
    options.ClaimActions.MapJsonKey("religion", "religion");
    #endregion


    #endregion

    options.TokenValidationParameters = new() // to apply claims on token
    {
        NameClaimType = "preferred_username",
        RoleClaimType = "role",
    };
});

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("AbleToFetch", AuthorizationPolicies.CanGetWathers());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
