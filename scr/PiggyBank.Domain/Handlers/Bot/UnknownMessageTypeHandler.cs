using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Bot;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Domain.Helpers;
using Telegram.Bot;

namespace PiggyBank.Domain.Handlers.Bot
{
    public class UnknownMessageTypeHandler : BaseHandler<UnknownMessageTypeCommand>
    {
        private readonly ITelegramBotClient _client;

        public UnknownMessageTypeHandler(DbContext context, UnknownMessageTypeCommand command, ITelegramBotClient client) : base(context, command)
            => _client = client;

        public override async Task Invoke(CancellationToken token)
        {
            var message = "Oppps! Unsupported type of message. Try again 😉";
            var startKeyboard = BotKeyboardHelper.GenerateStartKeyboard();
            await _client.SendTextMessageAsync(Command.ChatId, message, replyMarkup: startKeyboard, cancellationToken: token);
        }
    }
}