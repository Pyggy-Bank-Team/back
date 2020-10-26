using System.ComponentModel.DataAnnotations;

namespace PiggyBank.WebApi.Requests.Operations.Budget
{
    public class BudgetOperationRequestBase
    {
        [Range(1, 2)]
        public int AccountId { get; set; }

        [Range(1, 2)]
        public int CategoryId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Comment { get; set; }
    }
}