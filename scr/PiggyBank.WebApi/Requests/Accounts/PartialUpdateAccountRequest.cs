using PiggyBank.Common.Enums;

namespace PiggyBank.WebApi.Requests.Accounts
{
    public class PartialUpdateAccountRequest
    {
        public string Title { get; set; }

        public AccountType? Type { get; set; }
        
        // [Required]
        // public string Currency { get; set; }

        public decimal? Balance { get; set; }
        
        public bool? IsArchived { get; set; }
    }
}