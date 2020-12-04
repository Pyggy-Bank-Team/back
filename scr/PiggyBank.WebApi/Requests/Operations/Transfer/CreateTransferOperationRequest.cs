using System;

namespace PiggyBank.WebApi.Requests.Operations.Transfer
{
    public class CreateTransferOperationRequest : TransferOperationRequestBase
    {
        public DateTime? OperationDate { get; set; }
    }
}