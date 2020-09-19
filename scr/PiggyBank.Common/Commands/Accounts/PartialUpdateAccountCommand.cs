using PiggyBank.Common.Enums;

namespace PiggyBank.Common.Commands.Accounts
{
    public class PartialUpdateAccountCommand : BaseModifiedCommand
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public AccountType? Type { get; set; }

        public string Currency { get; set; }

        public decimal? Balance { get; set; }
        
        public bool? IsArchive { get; set; }
    }
}