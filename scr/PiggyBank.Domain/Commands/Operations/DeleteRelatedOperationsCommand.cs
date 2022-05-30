using MediatR;
using PiggyBank.Domain.Results.Operations;

namespace PiggyBank.Domain.Commands.Operations
{
    public class DeleteRelatedOperationsCommand : IRequest<DeleteRelatedOperationsResult>
    {
        public int AccountId { get; set; }
    }
}