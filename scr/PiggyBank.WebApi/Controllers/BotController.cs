using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
            var chatId = request.Message.Chat.Id.ToString();
            var session = HttpContext.Session;
            var savedJson = session.GetString(chatId);
            
            var operationSnapshot = await service.ProcessUpdateCommand(request, savedJson, token);

            if (!string.IsNullOrWhiteSpace(operationSnapshot))
            {
                if (session.Keys.Contains(chatId))
                    session.Remove(chatId);

                session.SetString(chatId, operationSnapshot);
            }
            
            return Ok();
        }
    }
}