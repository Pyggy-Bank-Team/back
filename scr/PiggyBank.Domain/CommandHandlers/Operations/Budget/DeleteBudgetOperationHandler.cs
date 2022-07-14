using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Commands.Accounts;
using Common.Commands.Operations.Budget;
using Common.Enums;
using Common.Results.Operations.Budget;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.CommandHandlers.Operations.Budget
{
    public class DeleteBudgetOperationHandler : IRequestHandler<DeleteBudgetOperationCommand, DeleteBudgetOperationResult>
    {
        private readonly IBudgetOperationRepository _repository;
        private readonly IMediator _mediator;

        public DeleteBudgetOperationHandler(IBudgetOperationRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<DeleteBudgetOperationResult> Handle(DeleteBudgetOperationCommand request, CancellationToken cancellationToken)
        {
            var operation = await _repository.GetAsync(request.ModifiedBy, request.Id, cancellationToken);

            if (operation == null)
                return new DeleteBudgetOperationResult { ErrorCode = ErrorCodes.NotFound };

            if (operation.IsDeleted)
                return new DeleteBudgetOperationResult();

            operation.IsDeleted = true;
            operation.ModifiedBy = request.ModifiedBy;
            operation.ModifiedOn = request.ModifiedOn;

            _ = await _repository.UpdateAsync(operation, cancellationToken);

            _ = await _mediator.Send(new ChangeBalanceCommand
            {
                Amount = operation.Category.Type == CategoryType.Income ? -operation.Amount : operation.Amount,
                AccountId = operation.AccountId,
                UserId = request.ModifiedBy
            }, cancellationToken);

            return new DeleteBudgetOperationResult();
        }
    }
}