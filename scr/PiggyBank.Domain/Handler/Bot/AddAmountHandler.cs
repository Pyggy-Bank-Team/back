using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Bot;
using PiggyBank.Common.Enums;
using PiggyBank.Model.Models.Entities;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace PiggyBank.Domain.Handler.Bot
{
    public class AddAmountHandler : BaseHandler<UpdateCommand>
    {
        private readonly ITelegramBotClient _client;
        private readonly BotOperation _operation;

        public AddAmountHandler(DbContext context, UpdateCommand command, ITelegramBotClient client, BotOperation operation) : base(context, command)
            => (_client, _operation) = (client, operation);

        public override async Task Invoke(CancellationToken token)
        {
            var receivedAmount = Command.Text;

            if (!decimal.TryParse(receivedAmount, out var amount))
            {
                var message = "Opps! Couldn't understand your amount. Please try different one.";
                await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken: token);
                return;
            }

            var accounts = GetRepository<Account>().Where(a => a.CreatedBy == _operation.CreatedBy && !a.IsArchived && !a.IsDeleted);

            if (!accounts.Any())
            {
                var message = "Couldn't find any accounts. Please add new account by PiggyBank app and try again.";
                await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken: token);
                return;
            }
            
            _operation.Amount = amount;
            _operation.Stage = CreationStage.One;
            _operation.ModifiedBy = Guid.Parse(Command.UserId);
            _operation.ModifiedOn = DateTime.UtcNow;

            GetRepository<BotOperation>().Update(_operation);

            //TODO
            var keys = new List<KeyboardButton[]>();
            foreach (var account in accounts.Take(30))
                keys.Add(new KeyboardButton[]{account.Title});
            
            var startKeyboard = new ReplyKeyboardMarkup(keys);
            await _client.SendTextMessageAsync(Command.ChatId, "Choose your accounts", replyMarkup: startKeyboard, cancellationToken: token);

            Result = _operation;
        }
    }
}