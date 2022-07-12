using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Commands.Accounts;
using Common.Commands.Operations.Budget;
using Common.Enums;
using Common.Queries;
using Common.Results.Models.Dto.Operations;
using Common.Results.Operations;
using MediatR;
using Newtonsoft.Json;
using PiggyBank.Domain.Models.Operations;
using PiggyBank.Model.Interfaces;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.CommandHandlers.Operations.Budget
{
    public class AddBudgetOperationHandler : IRequestHandler<AddBudgetOperationCommand, AddBudgetOperationResult>
    {
        private readonly IBudgetOperationRepository _repository;
        private readonly IMediator _mediator;

        public AddBudgetOperationHandler(IBudgetOperationRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<AddBudgetOperationResult> Handle(AddBudgetOperationCommand request, CancellationToken cancellationToken)
        {
            var account = await _mediator.Send(new GetAccountQuery { AccountId = request.AccountId, UserId = request.CreatedBy }, cancellationToken);

            if (account == null)
                return new AddBudgetOperationResult { ErrorCode = ErrorCodes.NotFound, Messages = new[] { "Account not found" } };

            var category = await _mediator.Send(new GetCategoryQuery { CategoryId = request.CategoryId, UserId = request.CreatedBy }, cancellationToken);

            if (category == null)
                return new AddBudgetOperationResult { ErrorCode = ErrorCodes.NotFound, Messages = new[] { "Category not found" } };

            var snapshot = new OperationSnapshot
            {
                CategoryType = category.Data.Type
            };

            var operation = new BudgetOperation
            {
                Amount = request.Amount,
                Type = OperationType.Budget,
                Comment = request.Comment,
                AccountId = request.AccountId,
                CategoryId = request.CategoryId,
                CreatedOn = request.CreatedOn,
                Snapshot = JsonConvert.SerializeObject(snapshot),
                OperationDate = request.OperationDate,
                CreatedBy = request.CreatedBy
            };
            
            _ = await _mediator.Send(new ChangeBalanceCommand
            {
                AccountId = account.Data.Id,
                Amount = category.Data.Type == CategoryType.Income ? operation.Amount : -operation.Amount,
                UserId = request.CreatedBy
            }, cancellationToken);

            var createdOperation = await _repository.AddAsync(operation, cancellationToken);
            return new AddBudgetOperationResult
            {
                Data = new BudgetDto
                {
                    Amount = createdOperation.Amount,
                    Comment = createdOperation.Comment,
                    Date = createdOperation.CreatedOn,
                    Id = createdOperation.Id,
                    Type = createdOperation.Type,
                    AccountId = createdOperation.AccountId,
                    CategoryId = createdOperation.CategoryId
                }
            };
        }
    }
}