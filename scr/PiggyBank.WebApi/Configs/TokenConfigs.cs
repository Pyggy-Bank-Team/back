namespace PiggyBank.WebApi.Configs
{
    public class TokenConfigs
    {
        public const string SectionName = "TokenConfigs";
        
        public string ClientSecret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}