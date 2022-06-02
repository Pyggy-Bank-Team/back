using System;

namespace Common.Commands.Operations.Transfer
{
    public class AddTransferOperationCommand : BaseCreateCommand
    {
        public int From { get; set; }

        public int To { get; set; }

        public decimal Amount { get; set; }

        public string Comment { get; set; }
        
        public DateTime OperationDate { get; set; }
    }
}
