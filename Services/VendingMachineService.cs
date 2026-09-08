namespace IntravisionTest.Services
{
    public class InsertCoinResult
    {
        public bool Success { get; set; }
        public int NewAmount { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class BuyDrinkResult
    {
        public bool Success { get; set; }
        public int NewAmount { get; set; }
        public Dictionary<int, int> Change { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }

    public class VendingMachineService
    {
        private readonly CoinService _coinService;
        private readonly DrinkService _drinkService;

        public VendingMachineService(CoinService coinService, DrinkService drinkService)
        {
            _coinService = coinService;
            _drinkService = drinkService;
        }

        public InsertCoinResult InsertCoin(int denomination, int currentAmount)
        {
            var coin = _coinService.GetCoinByDenomination(denomination);

            if (coin == null || coin.IsBlocked)
            {
                return new InsertCoinResult
                {
                    Success = false,
                    NewAmount = currentAmount,
                    ErrorMessage = "Монета не принимается автоматом"
                };
            }

            var newAmount = currentAmount + denomination;
            _coinService.AddCoin(denomination);

            return new InsertCoinResult { Success = true, NewAmount = newAmount };
        }

        public BuyDrinkResult BuyDrink(Guid drinkId, int currentAmount)
        {
            var drink = _drinkService.GetDrink(drinkId);

            if (drink == null || drink.Count <= 0)
                return new BuyDrinkResult { Success = false, NewAmount = currentAmount, ErrorMessage = "Напиток недоступен" };

            if (currentAmount < drink.Price)
                return new BuyDrinkResult { Success = false, NewAmount = currentAmount, ErrorMessage = "Недостаточно денег" };

            var changeAmount = (int)(currentAmount - drink.Price);
            var change = CalculateChange(changeAmount);

            if (change == null)
                return new BuyDrinkResult { Success = false, NewAmount = currentAmount, ErrorMessage = "Автомат не может выдать сдачу" };

            _drinkService.DecreaseDrinkCount(drinkId);

            foreach (var pair in change)
                _coinService.DecreaseCoinCount(pair.Key, pair.Value);

            return new BuyDrinkResult { Success = true, NewAmount = 0, Change = change };
        }

        private Dictionary<int, int>? CalculateChange(int amount)
        {
            var result = new Dictionary<int, int>();
            var denominations = new[] { 10, 5, 2, 1 };

            foreach (var denomination in denominations)
            {
                var coin = _coinService.GetCoinByDenomination(denomination);
                var available = coin?.Count ?? 0;

                var neededCount = amount / denomination;
                var usedCount = Math.Min(neededCount, available);

                if (usedCount > 0)
                {
                    result[denomination] = usedCount;
                    amount -= usedCount * denomination;
                }
            }

            return amount > 0 ? null : result;
        }
    }
}
