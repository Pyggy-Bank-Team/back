using MediatR;
using Common.Results.Accounts;

namespace Common.Commands.Accounts
{
    public class DeleteAccountCommand : BaseModifiedCommand, IRequest<DeleteAccountResult>
    {
        public int Id { get; set; }
    }
}