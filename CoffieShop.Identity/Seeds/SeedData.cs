using CoffieShop.Identity.Configs;
using CoffieShop.Identity.Context;
using CoffieShop.Identity.Identity;
using IdentityModel;
using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CoffieShop.Identity.Seeds
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<CoffieShopIdentityDbContext>(
                options => options.UseSqlServer(connectionString)
            );

            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<CoffieShopIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddOperationalDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName)
                        );
                }
            );
            services.AddConfigurationDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName)
                        );
                }
            );

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();


            var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            context.Database.Migrate();
            EnsureSeedData(context);

            var ctx = scope.ServiceProvider.GetService<CoffieShopIdentityDbContext>();
            ctx.Database.Migrate();
            EnsureUsers(scope);
        }

        private static void EnsureUsers(IServiceScope scope)
        {
            try
            {
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var _user = userMgr.FindByNameAsync("M2ri").Result;
                if (_user == null)
                {
                    _user = new ApplicationUser
                    {
                        UserName = "M2ri",
                        Email = "m.eltorri@gmail.com",
                        EmailConfirmed = true,
                        Firstname = "Mahmoud",
                        LastName = "El Torri"
                    };
                    var result = userMgr.CreateAsync(_user, "Pa$$w0rd").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result =
                        userMgr.AddClaimsAsync(
                            _user,
                            new Claim[]
                            {
                                new Claim(JwtClaimTypes.Name, "Mahmoud El Torri"),
                                new Claim(JwtClaimTypes.PreferredUserName, "MTorri"),
                                new Claim(JwtClaimTypes.FamilyName, "El Torri"),
                                new Claim(JwtClaimTypes.GivenName, "Mahmoud"),
                                new Claim(JwtClaimTypes.MiddleName, "Gaber"),
                                new Claim(JwtClaimTypes.Gender, "Male"),
                                new Claim(JwtClaimTypes.Locale, "en"),
                                new Claim(JwtClaimTypes.PhoneNumber, "0115931334"),
                                new Claim(JwtClaimTypes.WebSite, "www.M2ri.com"),
                                new Claim("role", "Admin"),
                                new Claim("country", "USA"),
                            }
                        ).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {

                foreach (var client in Config.Clients.ToList())
                {
                    try
                    {
                        var _client = client.ToEntity();
                        context.Clients.Add(_client);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes.ToList())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.ApiResources.ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
    }
}
