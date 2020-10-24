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
    }
}