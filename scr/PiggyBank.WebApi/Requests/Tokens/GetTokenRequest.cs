using System.ComponentModel.DataAnnotations;

namespace PiggyBank.WebApi.Requests.Tokens
{
    public class GetTokenRequest
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}