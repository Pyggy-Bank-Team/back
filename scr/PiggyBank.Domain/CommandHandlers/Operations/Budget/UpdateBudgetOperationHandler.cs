using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Commands.Accounts;
using Common.Commands.Operations.Budget;
using Common.Enums;
using Common.Queries;
using Common.Results.Models.Dto.Operations;
using Common.Results.Operations.Budget;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.CommandHandlers.Operations.Budget
{
    public class UpdateBudgetOperationHandler : IRequestHandler<UpdateBudgetOperationCommand, UpdateBudgetOperationResult>
    {
        private readonly IBudgetOperationRepository _repository;
        private readonly IMediator _mediator;

        public UpdateBudgetOperationHandler(IBudgetOperationRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }
        
        public async Task<UpdateBudgetOperationResult> Handle(UpdateBudgetOperationCommand request, CancellationToken cancellationToken)
        {
            var operation = await _repository.GetAsync(request.ModifiedBy, request.Id, cancellationToken);

            if (operation == null)
                return new UpdateBudgetOperationResult { ErrorCode = ErrorCodes.NotFound };

            var getAccountResult = await _mediator.Send(new GetAccountQuery { AccountId = request.AccountId, UserId = request.ModifiedBy }, cancellationToken);

            if (!getAccountResult.IsSuccess)
                return new UpdateBudgetOperationResult { ErrorCode = ErrorCodes.InvalidRequest, Messages = new[] { $"Account with {request.AccountId} is not found" } };

            var getCategoryResult = await _mediator.Send(new GetCategoryQuery { CategoryId = request.CategoryId, UserId = request.ModifiedBy }, cancellationToken);

            if (!getCategoryResult.IsSuccess)
                return new UpdateBudgetOperationResult { ErrorCode = ErrorCodes.InvalidRequest, Messages = new[] { $"Category with {request.CategoryId} is not found" } };

            //If the amount was changed then I undo the last change and 
            //Confirm the current amount
            if (operation.Amount != request.Amount)
            {
                _ = await _mediator.Send(new ChangeBalanceCommand
                {
                    Amount = operation.Category.Type == CategoryType.Income ? -operation.Amount : operation.Amount,
                    AccountId = operation.AccountId,
                    UserId = request.ModifiedBy
                }, cancellationToken);
                
                _ = await _mediator.Send(new ChangeBalanceCommand
                {
                    Amount = operation.Category.Type == CategoryType.Income ? request.Amount : -request.Amount,
                    AccountId = operation.AccountId,
                    UserId = request.ModifiedBy
                }, cancellationToken);
            }

            operation.Amount = request.Amount;
            operation.Comment = request.Comment;
            operation.CategoryId = request.CategoryId;
            operation.AccountId = request.AccountId;
            operation.ModifiedBy = request.ModifiedBy;
            operation.ModifiedOn = request.ModifiedOn;
            operation.OperationDate = request.OperationDate ?? operation.OperationDate;

            var budgetOperation = await _repository.UpdateAsync(operation, cancellationToken);
            return new UpdateBudgetOperationResult
            {
                Data = new BudgetDto
                {
                    Amount = budgetOperation.Amount,
                    Comment = budgetOperation.Comment,
                    Date = budgetOperation.OperationDate,
                    Id = budgetOperation.Id,
                    Type = budgetOperation.Type,
                    AccountId = budgetOperation.AccountId,
                    CategoryId = budgetOperation.CategoryId
                }
            };
        }
    }
}