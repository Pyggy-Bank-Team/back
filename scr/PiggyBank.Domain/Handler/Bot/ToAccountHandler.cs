using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Bot;
using PiggyBank.Common.Enums;
using PiggyBank.Domain.Helpers;
using PiggyBank.Model.Models.Entities;
using Telegram.Bot;

namespace PiggyBank.Domain.Handler.Bot
{
    public class ToAccountHandler : BaseHandler<UpdateCommand>
    {
        private readonly ITelegramBotClient _client;
        private readonly BotOperation _operation;

        public ToAccountHandler(DbContext context, UpdateCommand command,  ITelegramBotClient client, BotOperation operation) : base(context, command)
        {
            _client = client;
            _operation = operation;
        }

        public override async Task Invoke(CancellationToken token)
        {
            var accountRepository = GetRepository<Account>();
            
            var toAccount = await accountRepository.FirstOrDefaultAsync(a => a.Title == Command.Text, token);

            if (toAccount == null)
            {
                var message = "Couldn't find any accounts. Please add new account by PiggyBank app and try again.";
                await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken: token);
                return;
            }
            
            _operation.Stage = CreationStage.Done;
            _operation.ModifiedBy = Guid.Parse(Command.UserId);
            _operation.ModifiedOn = DateTime.UtcNow;
            _operation.ToAccountId = toAccount.Id;

            GetRepository<BotOperation>().Update(_operation);

            var operation = new TransferOperation
            {
                Amount = _operation.Amount.Value,
                Type = OperationType.Transfer,
                From = _operation.AccountId.Value,
                To = _operation.ToAccountId.Value,
                OperationDate = _operation.CreatedOn,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = _operation.CreatedBy
            };

            var fromAccount = await accountRepository.FirstOrDefaultAsync(a => a.Id == _operation.AccountId, token);

            fromAccount.ChangeBalance(-operation.Amount);
            toAccount.ChangeBalance(operation.Amount);

            accountRepository.UpdateRange(new[] { fromAccount, toAccount });

            await GetRepository<TransferOperation>().AddAsync(operation, token);
            
            var finalMessage = $"{fromAccount.Title} > {toAccount.Title} {operation.Amount} {fromAccount.Currency}";
            await _client.SendTextMessageAsync(Command.ChatId, finalMessage, replyMarkup:BotKeyboardHelper.GenerateStartKeyboard(), cancellationToken: token);
        }
    }
}