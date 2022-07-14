using Common.Results.Operations.Budget;
using MediatR;

namespace Common.Commands.Operations.Budget
{
    public class DeleteBudgetOperationsByAccountIdCommand : BaseModifiedCommand, IRequest<DeleteBudgetOperationsByAccountIdResult>
    {
        public int AccountId { get; set; }
    }
}