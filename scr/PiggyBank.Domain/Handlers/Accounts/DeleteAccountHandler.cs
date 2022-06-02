using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PiggyBank.Domain.Commands.Accounts;
using PiggyBank.Domain.Commands.Operations;
using PiggyBank.Domain.Results.Accounts;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.Handlers.Accounts
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, DeleteAccountResult>
    {
        private readonly IAccountRepository _repository;
        private readonly IMediator _mediator;

        public DeleteAccountHandler(IAccountRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<DeleteAccountResult> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetAsync(request.ModifiedBy, request.Id, cancellationToken);

            if (account == null || account.IsDeleted)
                return new DeleteAccountResult();

            account.IsDeleted = true;
            account.ModifiedBy = request.ModifiedBy;
            account.ModifiedOn = request.ModifiedOn;

            var deleteRelatedOperationsCommand = new DeleteRelatedOperationsCommand
            {
                AccountId = account.Id
            };
            
            var deleteRelatedOperationsResult = await _mediator.Send(deleteRelatedOperationsCommand, cancellationToken);

            if (!deleteRelatedOperationsResult.IsSuccess)
                return new DeleteAccountResult { ErrorCode = deleteRelatedOperationsResult.ErrorCode, Messages = deleteRelatedOperationsResult.Messages};
            
            var _ = await _repository.UpdateAsync(account, cancellationToken);

            return new DeleteAccountResult();
        }
    }
}