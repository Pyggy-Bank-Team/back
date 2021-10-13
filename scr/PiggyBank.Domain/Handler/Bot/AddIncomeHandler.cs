using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Enums;
using PiggyBank.Domain.Models.Operations;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PiggyBank.Domain.Handler.Bot
{
    public class AddIncomeHandler : BaseHandler<Message>
    {
        private readonly ITelegramBotClient _client;
        private readonly string _userId;

        public AddIncomeHandler(DbContext context, Message command, ITelegramBotClient client, string userId) : base(context, command)
        {
            _client = client;
            _userId = userId;
        }

        public override async Task Invoke(CancellationToken token)
        {
            var message = "Enter the transaction amount:";
            await _client.SendTextMessageAsync(Command.Chat.Id, message, cancellationToken: token);

            Result = new BotOperationSnapshot
            {
                ChatId = Command.Chat.Id,
                CreatedBy = _userId,
                Step = Step.Zero,
                Type = OperationType.Budget,
                CategoryType = CategoryType.Income
            };
        }
    }
}