using MediatR;
using Common.Results.Accounts;

namespace Common.Commands.Accounts
{
    public class DeleteAccountsCommand : BaseModifiedCommand, IRequest<DeleteAccountsResult>
    {
        public int[] Ids { get; set; }
    }
}