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
    public class AccountOperationHandler : BaseHandler<UpdateCommand>
    {
        private readonly ITelegramBotClient _client;
        private readonly BotOperation _operation;

        public AccountOperationHandler(DbContext context, UpdateCommand command, ITelegramBotClient client, BotOperation operation) : base(context, command)
        {
            _client = client;
            _operation = operation;
        }

        public override async Task Invoke(CancellationToken token)
        {
            var account = GetRepository<Account>().FirstOrDefaultAsync(a => a.Title == Command.Text, token);

            if (account == null)
            {
                var message = $"Couldn't find account with title {Command.Text}. Please try different account.";
                await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken: token);
                return;
            }
            
            _operation.Stage = CreationStage.Two;
            _operation.ModifiedBy = Guid.Parse(Command.UserId);
            _operation.ModifiedOn = DateTime.UtcNow;
            _operation.AccountId = account.Id;

            GetRepository<BotOperation>().Update(_operation);

            if (_operation.Type == OperationType.Budget)
            {
                var categories = GetRepository<Category>().Where(c => c.CreatedBy == _operation.CreatedBy && c.Type == _operation.CategoryType && !c.IsArchived && !c.IsDeleted);
                
                if (!categories.Any())
                {
                    var message = "Couldn't find any categories. Please add new category by PiggyBank app and try again.";
                    await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken: token);
                    return;
                }
                
                //TODO
                var keys = new List<KeyboardButton[]>();
                foreach (var a in categories.Take(30))
                    keys.Add(new KeyboardButton[]{a.Title});
            
                var startKeyboard = new ReplyKeyboardMarkup(keys, resizeKeyboard:true);
                await _client.SendTextMessageAsync(Command.ChatId, "Choose your accounts", replyMarkup: startKeyboard, cancellationToken: token);
            }
            else
            {
                var accounts = GetRepository<Account>().Where(a => a.CreatedBy == _operation.CreatedBy && !a.IsArchived && !a.IsDeleted);

                if (!accounts.Any())
                {
                    var message = "Couldn't find any accounts. Please add new account by PiggyBank app and try again.";
                    await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken: token);
                    return;
                }
                
                //TODO
                var keys = new List<KeyboardButton[]>();
                foreach (var a in accounts.Take(30))
                    keys.Add(new KeyboardButton[]{a.Title});
            
                var startKeyboard = new ReplyKeyboardMarkup(keys, resizeKeyboard:true);
                await _client.SendTextMessageAsync(Command.ChatId, "Choose your accounts", replyMarkup: startKeyboard, cancellationToken: token);
            }
        }
    }
}