using System.Threading;
using System.Threading.Tasks;
using Identity.Model;
using Identity.Model.Models;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Bot;
using PiggyBank.Domain.Helpers;
using Telegram.Bot;

namespace PiggyBank.Domain.Handler.Bot
{
    public class StartHandler : BaseHandler<UpdateCommand>
    {
        private readonly ITelegramBotClient _client;

        public StartHandler(IdentityContext context, UpdateCommand command,  ITelegramBotClient client) : base(context, command)
            =>_client = client;

        public override async Task Invoke(CancellationToken token)
        {
            var text = Command.Text;
            var splitText = text.Split(' ');

            if (splitText.Length != 2)
            {
                var message = "To start a conversation, install and register in the PiggyBank app for Windows https://www.microsoft.com/store/productId/9P4665KCNDC3 (link to bot authorization in App Settings).\n\n Any questions: support@piggybank.pro";
                await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken:token);
                return;
            }

            var userId = splitText[1];
            //TODO decode

            var identityRepository = GetRepository<ApplicationUser>();
            var user = await identityRepository.FirstOrDefaultAsync(u => u.Id == userId, token);

            if (user == null)
            {
                var message = "User not found 🤨. Try to re-install the telegram bot.";
                await _client.SendTextMessageAsync(Command.ChatId, message, cancellationToken:token);
                return;
            }

            user.ChatId = Command.ChatId;
            await SaveAsync();

            var startKeyboard = BotKeyboardHelper.GenerateStartKeyboard();
            await _client.SendTextMessageAsync(Command.ChatId, "Now you can create operations", replyMarkup: startKeyboard, cancellationToken: token);
        }
    }
}