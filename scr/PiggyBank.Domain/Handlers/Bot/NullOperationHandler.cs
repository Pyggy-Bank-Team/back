using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Bot;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Domain.Helpers;
using Telegram.Bot;

namespace PiggyBank.Domain.Handlers.Bot
{
    public class NullOperationHandler : BaseHandler<UpdateCommand>
    {
        private readonly ITelegramBotClient _client;

        public NullOperationHandler(DbContext context, UpdateCommand command, ITelegramBotClient client) : base(context, command)
        {
            _client = client;
        }

        public override async Task Invoke(CancellationToken token)
        {
            var startKeyboard = BotKeyboardHelper.GenerateStartKeyboard();
            await _client.SendTextMessageAsync(Command.ChatId, "The previous operation not found. Please, create new one.", replyMarkup: startKeyboard, cancellationToken: token);
        }
    }
}