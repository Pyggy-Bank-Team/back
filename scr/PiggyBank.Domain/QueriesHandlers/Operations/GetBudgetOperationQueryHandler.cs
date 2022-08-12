using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Queries;
using Common.Results.Models.Dto.Operations;
using Common.Results.Operations.Budget;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.QueriesHandlers.Operations
{
    public class GetBudgetOperationQueryHandler : IRequestHandler<GetBudgetOperationQuery, GetBudgetOperationResult>
    {
        private readonly IBudgetOperationRepository _repository;

        public GetBudgetOperationQueryHandler(IBudgetOperationRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetBudgetOperationResult> Handle(GetBudgetOperationQuery request, CancellationToken cancellationToken)
        {
            var operation = await _repository.GetAsync(request.UserId, request.OperationId, cancellationToken);

            if (operation == null)
                return new GetBudgetOperationResult { ErrorCode = ErrorCodes.NotFound };

            return new GetBudgetOperationResult
            {
                Data = new BudgetDto
                {
                    Amount = operation.Amount,
                    Comment = operation.Comment,
                    Date = operation.OperationDate,
                    Id = operation.Id,
                    Type = operation.Type,
                    AccountId = operation.AccountId,
                    CategoryId = operation.CategoryId
                }
            };
        }
    }
}