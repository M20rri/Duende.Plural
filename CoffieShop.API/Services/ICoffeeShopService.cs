using CoffieShop.API.Models;

namespace CoffieShop.API.Services
{
    public interface ICoffeeShopService
    {
        Task<List<CoffeeShopModel>> List();
    }
}
