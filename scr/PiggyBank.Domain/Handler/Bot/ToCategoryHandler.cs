using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PiggyBank.Common.Commands.Bot;
using PiggyBank.Common.Enums;
using PiggyBank.Domain.Helpers;
using PiggyBank.Domain.Models.Operations;
using PiggyBank.Model.Models.Entities;
using Telegram.Bot;

namespace PiggyBank.Domain.Handler.Bot
{
    public class ToCategoryHandler : BaseHandler<UpdateCommand>
    {
        private readonly ITelegramBotClient _client;
        private readonly BotOperation _botOperation;

        public ToCategoryHandler(DbContext context, UpdateCommand command, ITelegramBotClient client, BotOperation botOperation) : base(context, command)
        {
            _client = client;
            _botOperation = botOperation;
        }

        public override async Task Invoke(CancellationToken token)
        {
            var category = await GetRepository<Category>().FirstOrDefaultAsync(c => c.Title == Command.Text && c.Type == _botOperation.CategoryType, token);

            if (category == null)
            {
                var message = "Couldn't find any categories. To continue please add new category by PiggyBank app and try again.";
                await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken: token);
                return;
            }

            _botOperation.Stage = CreationStage.Done;
            _botOperation.ModifiedBy = Guid.Parse(Command.UserId);
            _botOperation.ModifiedOn = DateTime.UtcNow;
            _botOperation.CategoryId = category.Id;

            GetRepository<BotOperation>().Update(_botOperation);

            var account = await GetRepository<Account>().FirstOrDefaultAsync(a => a.Id == _botOperation.AccountId, token);

            if (account == null)
            {
                var message = "Couldn't find any accounts. To continue please add new category by PiggyBank app and try again.";
                await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken: token);
                return;
            }

            var snapshot = new OperationSnapshot
            {
                CategoryType = category.Type
            };

            var operation = new BudgetOperation
            {
                Amount = _botOperation.Amount.GetValueOrDefault(),
                Type = OperationType.Budget,
                AccountId = _botOperation.AccountId.GetValueOrDefault(),
                CategoryId = _botOperation.CategoryId.Value,
                CreatedOn = DateTime.UtcNow,
                Snapshot = JsonConvert.SerializeObject(snapshot),
                OperationDate = _botOperation.CreatedOn,
                CreatedBy = _botOperation.CreatedBy,
                Source = Source.Bot,
                BotOperationId = _botOperation.Id
            };

            account.ChangeBalance(category.Type == CategoryType.Income ? operation.Amount : -operation.Amount);

            GetRepository<Account>().Update(account);

            await GetRepository<BudgetOperation>().AddAsync(operation, token);

            var finalMessage = category.Type == CategoryType.Expense
                ? $"{account.Title} > {category.Title} -{operation.Amount} {account.Currency}"
                : $"{category.Title} > {account.Title} +{operation.Amount} {account.Currency}";
            await _client.SendTextMessageAsync(Command.ChatId, finalMessage, replyMarkup: BotKeyboardHelper.GenerateStartKeyboard(), cancellationToken: token);
        }
    }
}