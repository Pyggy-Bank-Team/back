using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Accounts;
using Common.Results.Accounts;
using MediatR;

namespace PiggyBank.Domain.CommandHandlers.Accounts
{
    public class DeleteAccountsHandler : IRequestHandler<DeleteAccountsCommand, DeleteAccountsResult>
    {
        private readonly IMediator _mediator;

        public DeleteAccountsHandler(IMediator mediator)
            => _mediator = mediator;

        public async Task<DeleteAccountsResult> Handle(DeleteAccountsCommand request, CancellationToken cancellationToken)
        {
            foreach (var accountId in request.Ids)
            {
                var deleteAccountCommand = new DeleteAccountCommand { Id = accountId, ModifiedBy = request.ModifiedBy, ModifiedOn = request.ModifiedOn };
                var deleteAccountResult = await _mediator.Send(deleteAccountCommand, cancellationToken);

                if (!deleteAccountResult.IsSuccess)
                    return new DeleteAccountsResult { ErrorCode = deleteAccountResult.ErrorCode, Messages = deleteAccountResult.Messages };
            }

            return new DeleteAccountsResult();
        }
    }
}