using System;
using Common.Results.Operations.Budget;
using MediatR;

namespace Common.Commands.Operations.Budget
{
    public class DeleteBudgetOperationCommand : BaseModifiedCommand, IRequest<DeleteBudgetOperationResult>
    {
        public int Id { get; set; }
    }
}