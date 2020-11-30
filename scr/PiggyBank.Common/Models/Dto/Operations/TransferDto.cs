namespace PiggyBank.Common.Models.Dto.Operations
{
    public class TransferDto : OperationBase
    {
        public string Comment { get; set; }

        public int FromId { get; set; }

        public int ToId { get; set; }
    }
}