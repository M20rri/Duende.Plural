using Marvin.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = "https://localhost:5001";
            options.Audience = "MarvinApi"; // ApiResource
            options.TokenValidationParameters = new()
            {
                NameClaimType = "given_name",
                RoleClaimType = "role",
                ValidTypes = new[] { "at+jwt" }
            };
        });


builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("AbleToFetch", AuthorizationPolicies.CanGetWathers());
    o.AddPolicy("AbleToRead", policy =>
    {
        policy.RequireClaim("scope", "Custom.MarvinApi.read");
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
