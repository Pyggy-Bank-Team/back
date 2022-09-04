using MediatR;
using Common.Enums;
using Common.Results.Accounts;

namespace Common.Commands.Accounts
{
    public class UpdateAccountCommand : BaseModifiedCommand, IRequest<UpdateAccountResult>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public AccountType Type { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }
        
        public bool IsArchived { get; set; }
    }
}
