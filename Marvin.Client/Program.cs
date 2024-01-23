using Marvin.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    options.ClaimActions.Remove("aud");
    options.ClaimActions.DeleteClaim("sid");
    options.ClaimActions.DeleteClaim("idp");
    options.ClaimActions.DeleteClaim("auth_time");
    options.ClaimActions.DeleteClaim("identityprovider");

    // Add ultra scope
    options.Scope.Add("MarvinApi.fullAccess");
    options.Scope.Add("idRoles");
    options.Scope.Add("idCountry");
    options.ClaimActions.MapJsonKey("role", "role");
    options.ClaimActions.MapJsonKey("country", "country");

    options.TokenValidationParameters = new() // to apply claims on token
    {
        NameClaimType = "given_name",
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
