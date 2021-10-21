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
        private readonly BotOperation _botOperation;

        public ToAccountHandler(DbContext context, UpdateCommand command,  ITelegramBotClient client, BotOperation botOperation) : base(context, command)
        {
            _client = client;
            _botOperation = botOperation;
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
            
            _botOperation.Stage = CreationStage.Done;
            _botOperation.ModifiedBy = Guid.Parse(Command.UserId);
            _botOperation.ModifiedOn = DateTime.UtcNow;
            _botOperation.ToAccountId = toAccount.Id;

            GetRepository<BotOperation>().Update(_botOperation);

            var operation = new TransferOperation
            {
                Amount = _botOperation.Amount.GetValueOrDefault(),
                Type = OperationType.Transfer,
                From = _botOperation.AccountId.GetValueOrDefault(),
                To = _botOperation.ToAccountId.Value,
                OperationDate = _botOperation.CreatedOn,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = _botOperation.CreatedBy,
                Source = Source.Bot,
                BotOperationId = _botOperation.Id
            };

            var fromAccount = await accountRepository.FirstOrDefaultAsync(a => a.Id == _botOperation.AccountId, token);

            fromAccount.ChangeBalance(-operation.Amount);
            toAccount.ChangeBalance(operation.Amount);

            accountRepository.UpdateRange(new[] { fromAccount, toAccount });

            await GetRepository<TransferOperation>().AddAsync(operation, token);
            
            var finalMessage = $"{fromAccount.Title} > {toAccount.Title} {operation.Amount} {fromAccount.Currency}";
            await _client.SendTextMessageAsync(Command.ChatId, finalMessage, replyMarkup:BotKeyboardHelper.GenerateStartKeyboard(), cancellationToken: token);
        }
    }
}