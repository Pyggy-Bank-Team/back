using System;
using Common.Results.Operations.Transfer;
using MediatR;

namespace Common.Commands.Operations.Transfer
{
    public class DeleteTransferOperationsByAccountIdCommand : BaseModifiedCommand, IRequest<DeleteTransferOperationsByAccountIdResult>
    {
        public int AccountId { get; set; }
        public Guid UserId { get; set; }
    }
}