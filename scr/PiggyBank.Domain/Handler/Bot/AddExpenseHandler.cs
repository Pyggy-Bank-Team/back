using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Model;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PiggyBank.Domain.Handler.Bot
{
    public class AddExpenseHandler : BaseHandler<Message>
    {
        private readonly ITelegramBotClient _client;

        public AddExpenseHandler(PiggyContext context, Message command, ITelegramBotClient client) : base(context, command)
            => _client = client;

        public override async Task Invoke(CancellationToken token)
        {
                var message = "Enter the transaction amount:";
                await _client.SendTextMessageAsync(Command.Chat.Id, message, cancellationToken:token);
        }
    }
}