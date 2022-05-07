using MediatR;
using PiggyBank.Common.Commands;
using PiggyBank.Common.Enums;
using PiggyBank.Domain.Results.Accounts;

namespace PiggyBank.Domain.Commands.Accounts
{
    public class AddAccountCommand : BaseCreateCommand, IRequest<AddAccountResult>
    {
        public string Title { get; set; }

        public AccountType Type { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public bool IsArchived { get; set; }
    }
}
