using MediatR;
using PiggyBank.Common.Commands;
using PiggyBank.Domain.Results.Accounts;

namespace PiggyBank.Domain.Commands.Accounts
{
    public class DeleteAccountsCommand : BaseModifiedCommand, IRequest<DeleteAccountsResult>
    {
        public int[] Ids { get; set; }
    }
}