using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace PiggyBank.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class TelegramController : ControllerBase
    {
        [HttpPost, Route("update")]
        public async Task<IActionResult> Update([FromBody]Update request)
        {
            return Ok();
        }
    }
}