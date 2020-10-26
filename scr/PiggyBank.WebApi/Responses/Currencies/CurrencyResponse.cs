namespace PiggyBank.WebApi.Responses.Currencies
{
    public class CurrencyResponse
    {
        public CurrencyResponse(string code, string symbol)
            => (Code, Symbol) = (code, symbol);
        
        public static CurrencyResponse[] GetAvailableCurrencies()
            => new[]
            {
                new CurrencyResponse("RUB", "₽"),
                new CurrencyResponse("BYN", "Br"),
                new CurrencyResponse("UAH","₴"),
                new CurrencyResponse("KZT","₸"),
                new CurrencyResponse("USD", "$"),
                new CurrencyResponse("EUR", "€"), 
            };
        
        public string Code { get; set; }
        public string Symbol { get; set; }
    }
}