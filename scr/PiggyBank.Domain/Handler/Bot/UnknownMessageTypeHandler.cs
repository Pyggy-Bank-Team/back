using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Domain.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PiggyBank.Domain.Handler.Bot
{
    public class UnknownMessageTypeHandler : BaseHandler<Update>
    {
        private readonly ITelegramBotClient _client;

        public UnknownMessageTypeHandler(DbContext context, Update command, ITelegramBotClient client) : base(context, command)
        {
            _client = client;
        }

        public override async Task Invoke(CancellationToken token)
        {
            var message = "Oppps! Unsupported type of message. Try again 😉";
            var startKeyboard = BotKeyboardHelper.GenerateStartKeyboard();
            await _client.SendTextMessageAsync(Command.Message.Chat.Id, message, replyMarkup: startKeyboard, cancellationToken: token);
        }
    }
}