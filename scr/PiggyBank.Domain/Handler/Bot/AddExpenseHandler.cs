using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Bot;
using PiggyBank.Common.Enums;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace PiggyBank.Domain.Handler.Bot
{
    public class AddExpenseHandler : BaseHandler<UpdateCommand>
    {
        private const string Message = "Enter the transaction amount:";

        private readonly ITelegramBotClient _client;

        public AddExpenseHandler(PiggyContext context, UpdateCommand command, ITelegramBotClient client) : base(context, command)
            => _client = client;

        public override async Task Invoke(CancellationToken token)
        {
            await GetRepository<BotOperation>().AddAsync(new BotOperation
            {
                ChatId = Command.ChatId,
                CreatedBy = Guid.Parse(Command.UserId),
                CreatedOn = DateTime.UtcNow,
                Stage = CreationStage.Zero,
                Type = OperationType.Budget,
                CategoryType = CategoryType.Expense
            }, token);

            await _client.SendTextMessageAsync(Command.ChatId, Message, replyMarkup: new ReplyKeyboardRemove(), cancellationToken: token);
        }
    }
}