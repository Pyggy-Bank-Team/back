using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace PiggyBank.WebApi.Requests.Accounts
{
    public class CreateAccountRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        public AccountType Type { get; set; }
        
        // [Required]
        // public string Currency { get; set; }

        [Required]
        public decimal Balance { get; set; }
        
        public bool IsArchived { get; set; }
    }
}