using Telegram.Bot.Types.ReplyMarkups;

namespace PiggyBank.Domain.Helpers
{
    public static class BotKeyboardHelper
    {
        public static ReplyKeyboardMarkup GenerateStartKeyboard()
            => new ReplyKeyboardMarkup(new[] { new KeyboardButton[] { "➖ Add expense" },  new KeyboardButton[] { "➕ Add income" },  new KeyboardButton[] { "🔁 Add transfer" } }, resizeKeyboard: true);
    }
}