using IntravisionTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntravisionTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CoinService _coinService;
        private readonly DrinkService _drinkService;
        private readonly VendingMachineService _vendingMachineService;

        public UserController(CoinService coinService, DrinkService drinkService, VendingMachineService vendingMachineService)
        {
            _coinService = coinService;
            _drinkService = drinkService;
            _vendingMachineService = vendingMachineService;
        }

        [HttpGet("coins")]
        public IActionResult GetCoins()
        {
            var coins = _coinService.GetAllCoins().Select(c => new { c.Denomination, c.IsBlocked });
            return Ok(coins);
        }

        [HttpGet("drinks")]
        public IActionResult GetDrinks()
        {
            var drinks = _drinkService.GetAllDrinks().Select(d => new { d.Id, d.Name, d.Price, d.Count, d.ImageUrl });
            return Ok(drinks);
        }

        [HttpGet("amount")]
        public IActionResult GetAmount()
        {
            var amount = HttpContext.Session.GetInt32("Amount") ?? 0;
            return Ok(new { amount });
        }

        [HttpPost("insert-coin")]
        public IActionResult InsertCoin([FromBody] InsertCoinRequest request)
        {
            var currentAmount = HttpContext.Session.GetInt32("Amount") ?? 0;
            var result = _vendingMachineService.InsertCoin(request.Denomination, currentAmount);

            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            HttpContext.Session.SetInt32("Amount", result.NewAmount);
            return Ok(new { amount = result.NewAmount });
        }

        [HttpPost("buy")]
        public IActionResult Buy([FromBody] BuyDrinkRequest request)
        {
            var currentAmount = HttpContext.Session.GetInt32("Amount") ?? 0;
            var result = _vendingMachineService.BuyDrink(request.DrinkId, currentAmount);

            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            HttpContext.Session.SetInt32("Amount", result.NewAmount);
            return Ok(new { change = result.Change });
        }
    }

    public class InsertCoinRequest
    {
        public int Denomination { get; set; }
    }

    public class BuyDrinkRequest
    {
        public Guid DrinkId { get; set; }
    }
}
