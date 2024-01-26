using CoffieShop.API.Services;
using CoffieShop.DataAccess.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CoffieShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.Authority = "https://localhost:5443";
    o.Audience = "CoffeeAPI";
    o.TokenValidationParameters = new()
    {
        NameClaimType = "given_name",
        RoleClaimType = "role",
        ValidTypes = new[] { "at+jwt" }
    };
});

builder.Services.AddAuthorization();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("PublicSecure", policy =>
//    policy.RequireClaim("client_id", "secret_client_id"));

//    options.AddPolicy("UserSecure", policy =>
//    policy.RequireClaim("roleType", "CanReaddata"));

//    options.AddPolicy("AdminSecure", policy =>
//    policy.RequireClaim("roleType", "CanUpdatedata"));
//});

builder.Services.AddScoped<ICoffeeShopService, CoffeeShopService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
