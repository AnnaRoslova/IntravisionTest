using IntravisionTest.Data;
using IntravisionTest.Models;

namespace IntravisionTest.Services
{
    public class DrinkService
    {
        private readonly AppDbContext _context;

        public DrinkService(AppDbContext context)
        {
            _context = context;
        }

        public List<Drink> GetAllDrinks() => _context.Drinks.ToList();

        public Drink? GetDrink(Guid id) => _context.Drinks.FirstOrDefault(d => d.Id == id);

        public void AddDrink(Drink drink)
        {
            _context.Drinks.Add(drink);
            _context.SaveChanges();
        }

        public void UpdateDrink(Guid id, string name, decimal price, int count, string imageUrl)
        {
            var drink = GetDrink(id);
            if (drink != null)
            {
                drink.Name = name;
                drink.Price = price;
                drink.Count = count;
                drink.ImageUrl = imageUrl;
                _context.SaveChanges();
            }
        }

        public void RemoveDrink(Guid id)
        {
            var drink = GetDrink(id);
            if (drink != null)
            {
                _context.Drinks.Remove(drink);
                _context.SaveChanges();
            }
        }

        public void DecreaseDrinkCount(Guid id)
        {
            var drink = GetDrink(id);
            if (drink != null && drink.Count > 0)
            {
                drink.Count--;
                _context.SaveChanges();
            }
        }
    }
}
