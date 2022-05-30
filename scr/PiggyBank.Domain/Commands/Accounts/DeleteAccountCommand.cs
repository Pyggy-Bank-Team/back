using MediatR;
using PiggyBank.Common.Commands;
using PiggyBank.Domain.Results.Accounts;

namespace PiggyBank.Domain.Commands.Accounts
{
    public class DeleteAccountCommand : BaseModifiedCommand, IRequest<DeleteAccountResult>
    {
        public int Id { get; set; }
    }
}