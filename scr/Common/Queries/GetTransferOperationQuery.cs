using System;
using Common.Results.Operations.Transfer;
using MediatR;

namespace Common.Queries
{
    public class GetTransferOperationQuery : IRequest<GetTransferOperationResult>
    {
        public int OperationId { get; set; }
        public Guid UserId { get; set; }
    }
}