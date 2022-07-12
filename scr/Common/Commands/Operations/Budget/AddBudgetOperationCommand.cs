using System;
using Common.Results.Operations;
using MediatR;

namespace Common.Commands.Operations.Budget
{
    public class AddBudgetOperationCommand : BaseCreateCommand, IRequest<AddBudgetOperationResult>
    {
        public int CategoryId { get; set; }

        public decimal Amount { get; set; }

        public int AccountId { get; set; }

        public string Comment { get; set; }
        
        public DateTime OperationDate { get; set; }
    }
}
