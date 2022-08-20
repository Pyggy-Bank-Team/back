using System;
using Common.Results.Operations.Transfer;
using MediatR;

namespace Common.Commands.Operations.Transfer
{
    public class UpdateTransferOperationCommand : BaseModifiedCommand, IRequest<UpdateTransferOperationResult>
    {
        public int Id { get; set; }
        
        public int From { get; set; }

        public int To { get; set; }

        public decimal Amount { get; set; }

        public string Comment { get; set; }

        public DateTime? OperationDate { get; set; }
    }
}