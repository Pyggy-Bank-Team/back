using MediatR;
using Common.Results.Operations;

namespace Common.Commands.Operations
{
    public class DeleteRelatedOperationsCommand : IRequest<DeleteRelatedOperationsResult>
    {
        public int AccountId { get; set; }
    }
}