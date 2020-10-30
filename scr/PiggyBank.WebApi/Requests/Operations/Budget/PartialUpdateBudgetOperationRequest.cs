using System.ComponentModel.DataAnnotations;

namespace PiggyBank.WebApi.Requests.Operations.Budget
{
    public class PartialUpdateBudgetOperationRequest 
    {
        [Range(1, int.MaxValue)] 
        public int? AccountId { get; set; }

        [Range(1, int.MaxValue)] 
        public int? CategoryId { get; set; }

        public decimal? Amount { get; set; }

        public string Comment { get; set; }
    }
}