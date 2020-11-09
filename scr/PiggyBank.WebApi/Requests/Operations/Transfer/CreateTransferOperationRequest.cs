using System;

namespace PiggyBank.WebApi.Requests.Operations.Transfer
{
    public class CreateTransferOperationRequest : TransferOperationRequestBase
    {
        public DateTime? CreatedOn { get; set; }
    }
}