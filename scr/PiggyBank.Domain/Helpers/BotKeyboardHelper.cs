using Telegram.Bot.Types.ReplyMarkups;

namespace PiggyBank.Domain.Helpers
{
    public static class BotKeyboardHelper
    {
        public static ReplyKeyboardMarkup GenerateStartKeyboard()
            => new ReplyKeyboardMarkup(new[] { new KeyboardButton[] { "➖ Add expense" },  new KeyboardButton[] { "➕ Add income" },  new KeyboardButton[] { "🔁 Add transfer" } }, resizeKeyboard: true);
        
        public static  ReplyKeyboardMarkup GenerateTryAgainKeyboard()
            => GenerateKeyboard("Try again");

        public static ReplyKeyboardMarkup GenerateTryAgainKeyboard(decimal amount)
            => GenerateKeyboard($"{amount}");
        
        private static ReplyKeyboardMarkup GenerateKeyboard(string buttonName)
         => new ReplyKeyboardMarkup(new[] { new KeyboardButton[] { buttonName }}, resizeKeyboard: true);
    }
}