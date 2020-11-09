using System;

namespace PiggyBank.WebApi.Requests.Operations.Budget
{
    public class CreateBudgetOperationRequest : BudgetOperationRequestBase
    {
        public DateTime? CreatedOn { get; set; }
    }
}