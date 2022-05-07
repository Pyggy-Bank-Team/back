using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Bot;
using Telegram.Bot;

namespace PiggyBank.Domain.Handlers.Bot
{
    public class SettingsHandler : BaseHandler<UpdateCommand>
    {
        private const string Message = "Bot does not currently support settings";

        private readonly ITelegramBotClient _client;

        public SettingsHandler(DbContext context, UpdateCommand command, ITelegramBotClient client) : base(context, command)
            => _client = client;

        public override Task Invoke(CancellationToken token)
            => _client.SendTextMessageAsync(Command.ChatId, Message, cancellationToken: token);
    }
}