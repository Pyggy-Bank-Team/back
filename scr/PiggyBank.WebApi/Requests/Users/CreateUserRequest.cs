using System.ComponentModel.DataAnnotations;

namespace PiggyBank.WebApi.Requests.Users
{
    public class CreateUserRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string CurrencyBase { get; set; }
        
        [Required, EmailAddress]
        public string Email { get; set; }
        
        public string Locale { get; set; }
    }
}