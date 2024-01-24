using CoffieShop.API.Models;
using CoffieShop.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace CoffieShop.API.Services
{
    public class CoffeeShopService : ICoffeeShopService
    {
        private readonly CoffieShopDbContext dbContext;

        public CoffeeShopService(CoffieShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<CoffeeShopModel>> List()
        {
            var coffeeShops = await (from shop in dbContext.CoffeeShops
                                     select new CoffeeShopModel()
                                     {
                                         Id = shop.Id,
                                         Name = shop.Name,
                                         OpeningHours = shop.OpeningHours,
                                         Address = shop.Address
                                     }).ToListAsync();

            return coffeeShops;
        }
    }
}
