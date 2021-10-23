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
    public class FromAccountHandler : BaseHandler<UpdateCommand>
    {
        private readonly ITelegramBotClient _client;
        private readonly BotOperation _operation;

        public FromAccountHandler(DbContext context, UpdateCommand command, ITelegramBotClient client, BotOperation operation) : base(context, command)
        {
            _client = client;
            _operation = operation;
        }

        public override async Task Invoke(CancellationToken token)
        {
            var account = await GetRepository<Account>().FirstOrDefaultAsync(a => a.Title == Command.Text, token);

            if (account == null)
            {
                var message = $"Couldn't find account with title {Command.Text}. Please try different account.";
                await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken: token);
                return;
            }

            _operation.Stage = CreationStage.CategoryOrAccountSelection;
            _operation.ModifiedBy = Guid.Parse(Command.UserId);
            _operation.ModifiedOn = DateTime.UtcNow;
            _operation.AccountId = account.Id;

            if (_operation.Type == OperationType.Budget)
            {
                var categories = GetRepository<Category>().Where(c => c.CreatedBy == _operation.CreatedBy && c.Type == _operation.CategoryType && !c.IsArchived && !c.IsDeleted);

                if (!await categories.AnyAsync(token))
                {
                    var message = "Couldn't find any categories.To continue please add new categories by PiggyBank app and try again.";
                    await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken: token);
                    return;
                }

                IEnumerable<KeyboardButton[]> BuildKeyboard(IReadOnlyList<Category> c)
                {
                    for (var i = 0; i < c.Count; i++)
                    {
                        if (i + 1 >= c.Count)
                            yield return new[] { new KeyboardButton(c[i].Title) };
                        else
                        {
                            yield return new[] { new KeyboardButton(c[i].Title), new KeyboardButton(c[i + 1].Title) };
                            i++;
                        }
                    }
                }

                var keys = BuildKeyboard(categories.ToArray());
                var keyboard = new ReplyKeyboardMarkup(keys, resizeKeyboard: true);
                await _client.SendTextMessageAsync(Command.ChatId, "Choose the category to which you want to transfer money", replyMarkup: keyboard, cancellationToken: token);
            }
            else
            {
                var accounts = GetRepository<Account>().Where(a => a.CreatedBy == _operation.CreatedBy && !a.IsArchived && !a.IsDeleted && a.Id != account.Id);

                if (!await accounts.AnyAsync(token))
                {
                    var message = "Couldn't find any accounts. Please add new account by PiggyBank app and try again.";
                    await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken: token);
                    return;
                }

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
                var keyboard = new ReplyKeyboardMarkup(keys, resizeKeyboard: true);
                await _client.SendTextMessageAsync(Command.ChatId, "Choose the account to which you want to transfer money", replyMarkup: keyboard, cancellationToken: token);
            }
        }
    }
}