using PiggyBank.Common.Commands;
using PiggyBank.Common.Enums;

namespace PiggyBank.Domain.Commands.Accounts
{
    public class AddAccountBatchCommand : BaseCreateCommand
    {
        public AccountItem[] Accounts { get; set; }
    }

    public class AccountItem
    {
        public string Title { get; set; }

        public AccountType Type { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public bool IsArchived { get; set; }
    }
}