using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Domain.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PiggyBank.Domain.Handler.Bot
{
    public class NullOperationHandler : BaseHandler<Message>
    {
        private readonly ITelegramBotClient _client;

        public NullOperationHandler(DbContext context, Message command, ITelegramBotClient client) : base(context, command)
        {
            _client = client;
        }

        public override async Task Invoke(CancellationToken token)
        {
            var startKeyboard = BotKeyboardHelper.GenerateStartKeyboard();
            await _client.SendTextMessageAsync(Command.Chat.Id, "The previous operation not found. Please, create new one.", replyMarkup: startKeyboard, cancellationToken: token);
        }
    }
}