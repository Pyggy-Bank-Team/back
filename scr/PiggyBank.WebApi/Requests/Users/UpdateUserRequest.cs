namespace PiggyBank.WebApi.Requests.Users
{
    public class UpdateUserRequest
    {
        public string NewCurrency { get; set; }
        
        public string Email { get; set; }
    }
}