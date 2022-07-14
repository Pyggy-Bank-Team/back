using Common.Results.Operations.Transfer;
using MediatR;

namespace Common.Commands.Operations.Transfer
{
    public class DeleteTransferOperationCommand : BaseModifiedCommand, IRequest<DeleteTransferOperationResult>
    {
        public int Id { get; set; }
    }
}