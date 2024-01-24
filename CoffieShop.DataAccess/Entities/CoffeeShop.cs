namespace CoffieShop.DataAccess.Entities
{
    public class CoffeeShop
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string OpeningHours { get; set; } = default!;
        public string Address { get; set; } = default!;
    }
}
