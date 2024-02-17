using CoffieShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffieShop.DataAccess.Context
{
    public class CoffieShopDbContext : DbContext
    {
        public CoffieShopDbContext(DbContextOptions<CoffieShopDbContext> options)
            : base(options)
        { }

        public DbSet<CoffeeShop> CoffeeShops { get; set; }
    }
}
