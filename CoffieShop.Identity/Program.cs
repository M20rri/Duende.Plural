using CoffieShop.Identity.Configs;
using CoffieShop.Identity.Context;
using CoffieShop.Identity.Identity;
using CoffieShop.Identity.Seeds;
using CoffieShop.Identity.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var seed = args.Contains("/seed");
if (seed)
{
    args = args.Except(new[] { "/seed" }).ToArray();
}


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var assembly = typeof(Program).Assembly.GetName().Name;
var defaultConnString = builder.Configuration.GetConnectionString("DefaultConnection");

if (seed)
{
    SeedData.EnsureSeedData(defaultConnString);
}

builder.Services.AddAuthorization();
builder.Services.AddDbContext<CoffieShopIdentityDbContext>(options =>
    options.UseSqlServer(defaultConnString,
        b => b.MigrationsAssembly(assembly)));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<CoffieShopIdentityDbContext>()
    .AddDefaultTokenProviders();

//builder.Services.AddIdentityServer(options =>
//{
//    options.EmitStaticAudienceClaim = true;
//})
//    .AddAspNetIdentity<ApplicationUser>()
//    .AddConfigurationStore(options =>
//    {
//        options.ConfigureDbContext = b =>
//        b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
//    })
//    .AddOperationalStore(options =>
//    {
//        options.ConfigureDbContext = b =>
//        b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
//    });
//.AddProfileService<ProfileService>()
//.AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();

builder.Services.AddIdentityServer(options =>
{
    options.EmitStaticAudienceClaim = true;
})
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddTestUsers(ConfigUser.Users)
    .AddDeveloperSigningCredential();


var app = builder.Build();
app.UseAuthentication();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.MapControllers();

app.Run();
