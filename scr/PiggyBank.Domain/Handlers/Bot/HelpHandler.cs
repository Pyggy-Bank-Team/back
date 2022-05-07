using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Bot;
using Telegram.Bot;

namespace PiggyBank.Domain.Handlers.Bot
{
    public class HelpHandler : BaseHandler<UpdateCommand>
    {
        private const string Message = "What I can do:\n- Create different types of operation: expense, income and transfer";

        private readonly ITelegramBotClient _client;

        public HelpHandler(DbContext context, UpdateCommand command, ITelegramBotClient client) : base(context, command)
            => _client = client;

        public override Task Invoke(CancellationToken token)
            => _client.SendTextMessageAsync(Command.ChatId, Message, cancellationToken: token);
    }
}