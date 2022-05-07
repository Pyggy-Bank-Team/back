using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.Common.Commands.Bot;
using Serilog;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PiggyBank.WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] //This attribute hide this controller in Swagger
    [ApiController, Route("api/[controller]")]
    public class BotController : ControllerBase
    {
        // [HttpPost, Route("update")]
        // public async Task<IActionResult> Update([FromServices] IBotService service, [FromServices] ILogger logger, [FromBody] Update request, CancellationToken token)
        // {
        //     if (request.Type != UpdateType.Message)
        //     {
        //         logger.Warning("Unknown request type. Actual request type: {RequestType}", request.Type);
        //         return Ok();
        //     }
        //
        //     var command = new UpdateCommand
        //     {
        //         Text = request.Message.Text,
        //         ChatId = request.Message.Chat.Id
        //     };
        //
        //     await service.UpdateProcessing(command, token);
        //
        //     return Ok();
        // }
    }
}