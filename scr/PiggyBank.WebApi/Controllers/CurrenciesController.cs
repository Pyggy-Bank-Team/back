using Microsoft.AspNetCore.Mvc;
using PiggyBank.WebApi.Responses.Currencies;

namespace PiggyBank.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CurrenciesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
            => Ok(CurrencyResponse.GetAvailableCurrencies());
    }
}