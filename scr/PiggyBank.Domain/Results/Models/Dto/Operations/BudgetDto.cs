namespace PiggyBank.Common.Models.Dto.Operations
{
    public class BudgetDto : OperationBase
    {
        public int AccountId { get; set; }

        public int CategoryId { get; set; }
    }
}