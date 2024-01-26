// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Eltizam.Identity.Service.Data;
using Eltizam.Identity.Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Eltizam.Identity.Service
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var alice = userMgr.FindByNameAsync("M2ri").Result;
                    if (alice == null)
                    {
                        alice = new ApplicationUser
                        {
                            UserName = "M2ri",
                            Email = "m.eltorri@gmail.com",
                            EmailConfirmed = true,
                            Firstname = "Mahmoud",
                            LastName = "El Torri"
                        };
                        var result = userMgr.CreateAsync(alice, "Pa$$w0rd").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(alice, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "Mahmoud El Torri"),
                                new Claim(JwtClaimTypes.PreferredUserName, "MTorri"),
                                new Claim(JwtClaimTypes.FamilyName, "El Torri"),
                                new Claim(JwtClaimTypes.GivenName, "Mahmoud"),
                                new Claim(JwtClaimTypes.MiddleName, "Gaber"),
                                new Claim(JwtClaimTypes.WebSite, "www.M2ri.com"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("M2ri created");
                    }
                    else
                    {
                        Log.Debug("M2ri already exists");
                    }

                }
            }
        }
    }
}
