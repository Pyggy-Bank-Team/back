using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Accounts;
using Common.Commands.Operations.Budget;
using Common.Commands.Operations.Transfer;
using Common.Results.Accounts;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.CommandHandlers.Accounts
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
            
            var _ = await _repository.UpdateAsync(account, cancellationToken);

            var deleteTransferOperationsCommand = new DeleteTransferOperationsByAccountIdCommand
            {
                AccountId = account.Id,
                ModifiedBy = request.ModifiedBy,
                ModifiedOn = request.ModifiedOn
            };
            
            var deleteTransferOperationsResult = await _mediator.Send(deleteTransferOperationsCommand, cancellationToken);

            if (!deleteTransferOperationsResult.IsSuccess)
                return new DeleteAccountResult { ErrorCode = deleteTransferOperationsResult.ErrorCode, Messages = deleteTransferOperationsResult.Messages};
            
            var deleteBudgetOperationsCommand = new DeleteBudgetOperationsByAccountIdCommand
            {
                AccountId = account.Id,
                ModifiedBy = request.ModifiedBy,
                ModifiedOn = request.ModifiedOn
            };
            
            var deleteBudgetOperationsResult = await _mediator.Send(deleteBudgetOperationsCommand, cancellationToken);

            if (!deleteBudgetOperationsResult.IsSuccess)
                return new DeleteAccountResult { ErrorCode = deleteBudgetOperationsResult.ErrorCode, Messages = deleteBudgetOperationsResult.Messages};

            return new DeleteAccountResult();
        }
    }
}