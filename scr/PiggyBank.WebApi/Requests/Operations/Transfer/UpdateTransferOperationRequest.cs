using System;

namespace PiggyBank.WebApi.Requests.Operations.Transfer
{
    public class UpdateTransferOperationRequest : TransferOperationRequestBase
    {
        public DateTime? OperationDate { get; set; }
    }
}