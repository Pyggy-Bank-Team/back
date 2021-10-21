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
    public class AmountInputHandler : BaseHandler<UpdateCommand>
    {
        private readonly ITelegramBotClient _client;
        private readonly BotOperation _operation;

        public AmountInputHandler(DbContext context, UpdateCommand command, ITelegramBotClient client, BotOperation operation) : base(context, command)
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

            if (!await accounts.AnyAsync(token))
            {
                var message = "Couldn't find any accounts.To continue please add new accounts by PiggyBank app and try again.";
                await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken: token);
                return;
            }
            
            _operation.Amount = amount;
            _operation.Stage = CreationStage.AccountSelection;
            _operation.ModifiedBy = Guid.Parse(Command.UserId);
            _operation.ModifiedOn = DateTime.UtcNow;

            GetRepository<BotOperation>().Update(_operation);

            IEnumerable<KeyboardButton[]> BuildKeyboard(IReadOnlyList<Account> a)
            {
                for (var i = 0; i < a.Count; i++)
                {
                    if (i + 1 >= a.Count)
                        yield return new[] { new KeyboardButton(a[i].Title) };
                    else
                    {
                        yield return new[] { new KeyboardButton(a[i].Title), new KeyboardButton(a[i + 1].Title) };
                        i++;
                    }
                }
            }

            var keys = BuildKeyboard(accounts.ToArray());
            var keyboard = new ReplyKeyboardMarkup(keys, resizeKeyboard:true);
            await _client.SendTextMessageAsync(Command.ChatId, "Choose the account from which you want to transfer money", replyMarkup: keyboard, cancellationToken: token);
        }
    }
}