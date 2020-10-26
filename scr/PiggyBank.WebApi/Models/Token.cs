namespace PiggyBank.WebApi.Models
{
    public class Token
    {
        public Token() { }

        public Token(string value)
            => Value = value;

        public string ErrorType { get; set; }
        public string Value { get; set; }

        public static implicit operator bool(Token token)
            => string.IsNullOrEmpty(token.ErrorType) && !string.IsNullOrEmpty(token.Value);
    }
}