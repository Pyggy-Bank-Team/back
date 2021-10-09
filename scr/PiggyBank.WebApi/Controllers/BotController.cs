using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.Common.Interfaces;
using Telegram.Bot.Types;

namespace PiggyBank.WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController, Route("api/[controller]")]
    public class BotController : ControllerBase
    {
        [HttpPost, Route("update")]
        public async Task<IActionResult> Update([FromServices]IBotService service, [FromBody]Update request, CancellationToken token)
        {
            await service.ProcessUpdateCommand(request, HttpContext.Session, token);
            return Ok();
        }
    }
}