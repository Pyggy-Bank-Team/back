namespace PiggyBank.Common.Commands.Operations.Transfer
{
    public class UpdateTransferOperationCommand
    {
        public int Id { get; set; }
        
        public int From { get; set; }

        public int To { get; set; }

        public decimal Amount { get; set; }

        public string Comment { get; set; }
    }
}