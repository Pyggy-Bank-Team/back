namespace PiggyBank.Common.Models.Dto.Operations
{
    public class BudgetDto : OperationBase
    {
        public string Comment { get; set; }

        public int AccountId { get; set; }

        public int CategoryId { get; set; }
    }
}