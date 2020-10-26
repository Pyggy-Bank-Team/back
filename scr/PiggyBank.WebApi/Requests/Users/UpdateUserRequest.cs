using System.ComponentModel.DataAnnotations;

namespace PiggyBank.WebApi.Requests.Users
{
    public class UpdateUserRequest
    {
        [Required]
        public string NewCurrency { get; set; }
        
        public string Email { get; set; }
    }
}