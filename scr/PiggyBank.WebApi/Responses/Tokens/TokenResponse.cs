namespace PiggyBank.WebApi.Responses.Tokens
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public long ExpiresIn { get; set; }
        public string TokenType { get; set; }
    }
}