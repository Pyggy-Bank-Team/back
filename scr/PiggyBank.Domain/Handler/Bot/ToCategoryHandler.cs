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
        private readonly BotOperation _operation;

        public ToCategoryHandler(DbContext context, UpdateCommand command, ITelegramBotClient client, BotOperation operation) : base(context, command)
        {
            _client = client;
            _operation = operation;
        }

        public override async Task Invoke(CancellationToken token)
        {
            var category = await GetRepository<Category>().FirstOrDefaultAsync(c => c.Title == Command.Text && c.Type == _operation.CategoryType, token);

            if (category == null)
            {
                var message = "Couldn't find any categories. To continue please add new category by PiggyBank app and try again.";
                await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken: token);
                return;
            }

            _operation.Stage = CreationStage.Done;
            _operation.ModifiedBy = Guid.Parse(Command.UserId);
            _operation.ModifiedOn = DateTime.UtcNow;
            _operation.CategoryId = category.Id;

            GetRepository<BotOperation>().Update(_operation);

            var account = await GetRepository<Account>().FirstOrDefaultAsync(a => a.Id == _operation.AccountId, token);

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
                Amount = _operation.Amount.Value,
                Type = OperationType.Budget,
                AccountId = _operation.AccountId.Value,
                CategoryId = _operation.CategoryId.Value,
                CreatedOn = DateTime.UtcNow,
                Snapshot = JsonConvert.SerializeObject(snapshot),
                OperationDate = _operation.CreatedOn,
                CreatedBy = _operation.CreatedBy,
                Source = Source.Bot
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