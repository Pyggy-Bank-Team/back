namespace PiggyBank.WebApi.Options
{
    public class BotOptions
    {
        public const string BotSection = "BotOptions";

        public string Token { get; set; }
        public string  ServerUrl { get; set; }
    }
}