using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PiggyBank.WebApi.Requests.Tokens
{
    public class GetTokenRequest
    {
        [Required(ErrorMessage = "UserName is required")]
        [JsonProperty("userName")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}