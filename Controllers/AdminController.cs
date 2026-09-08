using IntravisionTest.Models;
using IntravisionTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntravisionTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly CoinService _coinService;
        private readonly DrinkService _drinkService;
        private readonly IConfiguration _configuration;

        public AdminController(CoinService coinService, DrinkService drinkService, IConfiguration configuration)
        {
            _coinService = coinService;
            _drinkService = drinkService;
            _configuration = configuration;
        }

        private bool IsAuthorized()
        {
            var expectedKey = _configuration["AdminSecretKey"];
            var providedKey = Request.Query["key"].ToString();
            return !string.IsNullOrEmpty(expectedKey) && providedKey == expectedKey;
        }

        [HttpGet("coins")]
        public IActionResult GetCoins()
        {
            if (!IsAuthorized()) return Unauthorized();
            return Ok(_coinService.GetAllCoins());
        }

        [HttpPut("coins/{denomination}/count")]
        public IActionResult SetCoinCount(int denomination, [FromBody] SetCountRequest request)
        {
            if (!IsAuthorized()) return Unauthorized();
            _coinService.SetCoinCount(denomination, request.Count);
            return Ok();
        }

        [HttpPut("coins/{denomination}/blocked")]
        public IActionResult SetCoinBlocked(int denomination, [FromBody] SetBlockedRequest request)
        {
            if (!IsAuthorized()) return Unauthorized();
            _coinService.SetCoinBlocked(denomination, request.IsBlocked);
            return Ok();
        }

        [HttpGet("drinks")]
        public IActionResult GetDrinks()
        {
            if (!IsAuthorized()) return Unauthorized();
            return Ok(_drinkService.GetAllDrinks());
        }

        [HttpPost("drinks")]
        public IActionResult AddDrink([FromBody] DrinkDto dto)
        {
            if (!IsAuthorized()) return Unauthorized();

            var drink = new Drink
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Price = dto.Price,
                Count = dto.Count,
                ImageUrl = dto.ImageUrl
            };

            _drinkService.AddDrink(drink);
            return Ok(drink);
        }

        [HttpPut("drinks/{id}")]
        public IActionResult UpdateDrink(Guid id, [FromBody] DrinkDto dto)
        {
            if (!IsAuthorized()) return Unauthorized();
            _drinkService.UpdateDrink(id, dto.Name, dto.Price, dto.Count, dto.ImageUrl);
            return Ok();
        }

        [HttpDelete("drinks/{id}")]
        public IActionResult RemoveDrink(Guid id)
        {
            if (!IsAuthorized()) return Unauthorized();
            _drinkService.RemoveDrink(id);
            return Ok();
        }
        [HttpPost("drinks/import")]
        public IActionResult ImportDrinks([FromBody] List<DrinkDto> dtos)
        {
            if (!IsAuthorized()) return Unauthorized();

            foreach (var dto in dtos)
            {
                var drink = new Drink
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Price = dto.Price,
                    Count = dto.Count,
                    ImageUrl = dto.ImageUrl
                };
                _drinkService.AddDrink(drink);
            }

            return Ok(new { imported = dtos.Count });
        }
    }

    public class SetCountRequest { public int Count { get; set; } }
    public class SetBlockedRequest { public bool IsBlocked { get; set; } }
    public class DrinkDto
    {
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string ImageUrl { get; set; } = "";
    }
}
