using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Operations;
using Common.Results.Operations;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.CommandHandlers.Operations
{
    public class DeleteRelatedOperationsHandler : IRequestHandler<DeleteRelatedOperationsCommand, DeleteRelatedOperationsResult>
    {
        private readonly IBudgetOperationRepository _repository;

        public DeleteRelatedOperationsHandler(IBudgetOperationRepository repository)
            => _repository = repository;

        public async Task<DeleteRelatedOperationsResult> Handle(DeleteRelatedOperationsCommand request, CancellationToken cancellationToken)
        {
            foreach (var budgetOperation in await _repository.GetBudgetOperationsAsync(request.AccountId, cancellationToken))
            {
                budgetOperation.IsDeleted = true;
                budgetOperation.ModifiedBy = request.ModifiedBy;
                budgetOperation.ModifiedOn = request.ModifiedOn;
                await _repository.UpdateBudgetOperationAsync(budgetOperation, cancellationToken);
            }
            
            foreach (var transferOperation in await _repository.GetTransferOperationsAsync(request.AccountId, cancellationToken))
            {
                transferOperation.IsDeleted = true;
                transferOperation.ModifiedBy = request.ModifiedBy;
                transferOperation.ModifiedOn = request.ModifiedOn;
                await _repository.UpdateTransferOperationAsync(transferOperation, cancellationToken);
            }

            return new DeleteRelatedOperationsResult();
        }
    }
}