namespace PiggyBank.WebApi.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse(string type, string message)
            => (Type, Message) = (type, message);
        
        public string Type { get; set; }
        public string Message { get; set; }
    }
}