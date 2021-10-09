using System.Threading;
using System.Threading.Tasks;
using Identity.Model;
using Identity.Model.Models;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Domain.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PiggyBank.Domain.Handler.Bot
{
    public class StartHandler : BaseHandler<Message>
    {
        private readonly ITelegramBotClient _client;

        public StartHandler(IdentityContext context, Message command,  ITelegramBotClient client) : base(context, command)
            =>_client = client;

        public override async Task Invoke(CancellationToken token)
        {
            var text = Command.Text;
            var splitText = text.Split(' ');

            if (splitText.Length != 2)
            {
                var message = "To start a conversation, install and register in the PiggyBank app for Windows https://www.microsoft.com/store/productId/9P4665KCNDC3 (link to bot authorization in App Settings).\n\n Any questions: support@piggybank.pro";
                await _client.SendTextMessageAsync(Command.Chat.Id, message, cancellationToken:token);
                return;
            }

            var userId = splitText[1];
            //TODO decode

            var identityRepository = GetRepository<ApplicationUser>();
            var user = await identityRepository.FirstAsync(u => u.Id == userId, token);

            if (user == null)
            {
                var message = "User not found 🤨. Try to re-install the telegram bot.";
                await _client.SendTextMessageAsync(Command.Chat.Id, message, cancellationToken:token);
                return;
            }

            user.ChatId = Command.Chat.Id;
            await SaveAsync();

            var startKeyboard = BotKeyboardHelper.GenerateStartKeyboard();
            await _client.SendTextMessageAsync(Command.Chat.Id, "Now you can create operations", replyMarkup: startKeyboard, cancellationToken: token);
        }
    }
}