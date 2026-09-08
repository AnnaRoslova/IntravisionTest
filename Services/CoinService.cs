using IntravisionTest.Data;
using IntravisionTest.Models;

namespace IntravisionTest.Services
{
    public class CoinService
    {
        private readonly AppDbContext _context;

        public CoinService(AppDbContext context)
        {
            _context = context;
        }

        public List<Coin> GetAllCoins() => _context.Coins.ToList();

        public Coin? GetCoinByDenomination(int denomination) =>
            _context.Coins.FirstOrDefault(c => c.Denomination == denomination);

        public void AddCoin(int denomination)
        {
            var coin = GetCoinByDenomination(denomination);
            if (coin != null)
            {
                coin.Count++;
                _context.SaveChanges();
            }
        }

        public void DecreaseCoinCount(int denomination, int amount)
        {
            var coin = GetCoinByDenomination(denomination);
            if (coin != null)
            {
                coin.Count -= amount;
                _context.SaveChanges();
            }
        }

        public void SetCoinCount(int denomination, int newCount)
        {
            var coin = GetCoinByDenomination(denomination);
            if (coin != null)
            {
                coin.Count = newCount;
                _context.SaveChanges();
            }
        }

        public void SetCoinBlocked(int denomination, bool isBlocked)
        {
            var coin = GetCoinByDenomination(denomination);
            if (coin != null)
            {
                coin.IsBlocked = isBlocked;
                _context.SaveChanges();
            }
        }
    }
}
