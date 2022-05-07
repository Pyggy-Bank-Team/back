using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Bot;
using PiggyBank.Common.Enums;
using PiggyBank.Model.Models.Entities;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace PiggyBank.Domain.Handlers.Bot
{
    public class AddIncomeHandler : BaseHandler<UpdateCommand>
    {
        private const string Message = "Enter the transaction amount:";

        private readonly ITelegramBotClient _client;

        public AddIncomeHandler(DbContext context, UpdateCommand command, ITelegramBotClient client) : base(context, command)
            => _client = client;

        public override async Task Invoke(CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(Command.UserId))
            {
                var message = "Oops! Seems like someone already connected the bot. To connect the bot open `Settings` and  click on `Connect` to the bot at PiggyBank App.";
                await _client.SendTextMessageAsync(Command.ChatId, message, replyMarkup: new ReplyKeyboardRemove(), cancellationToken: token);
                return;
            }
            
            await GetRepository<BotOperation>().AddAsync(new BotOperation
            {
                ChatId = Command.ChatId,
                CreatedBy = Guid.Parse(Command.UserId),
                CreatedOn = DateTime.UtcNow,
                Stage = CreationStage.AmountInput,
                Type = OperationType.Budget,
                CategoryType = CategoryType.Income
            }, token);

            await _client.SendTextMessageAsync(Command.ChatId, Message, replyMarkup: new ReplyKeyboardRemove(), cancellationToken: token);
        }
    }
}