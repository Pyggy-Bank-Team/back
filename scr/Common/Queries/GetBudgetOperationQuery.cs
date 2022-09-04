using System;
using Common.Results.Operations.Budget;
using MediatR;

namespace Common.Queries
{
    public class GetBudgetOperationQuery : IRequest<GetBudgetOperationResult>
    {
        public int OperationId { get; set; }
        public Guid UserId { get; set; }
    }
}