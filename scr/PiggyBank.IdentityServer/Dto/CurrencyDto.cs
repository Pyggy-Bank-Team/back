namespace PiggyBank.IdentityServer.Dto
{
    public class CurrencyDto
    {
        public CurrencyDto(string code, string symbol)
            => (Code, Symbol) = (code, symbol);
        
        public static CurrencyDto[] GetAvailableCurrencies()
            => new[]
            {
                new CurrencyDto("RUB", "₽"),
                new CurrencyDto("BYN", "Br"),
                new CurrencyDto("UAH","₴"),
                new CurrencyDto("KZT","₸"),
                new CurrencyDto("USD", "$"),
                new CurrencyDto("EUR", "€"), 
            };
        
        public string Code { get; set; }
        public string Symbol { get; set; }
    }
}