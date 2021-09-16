using System;

namespace PiggyBank.WebApi.Requests.Operations.Budget
{
    public class UpdateBudgetOperationRequest : BudgetOperationRequestBase
    {
        public DateTime? OperationDate { get; set; }
    }
}