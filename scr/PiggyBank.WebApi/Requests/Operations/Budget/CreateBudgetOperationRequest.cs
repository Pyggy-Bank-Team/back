using System;

namespace PiggyBank.WebApi.Requests.Operations.Budget
{
    public class CreateBudgetOperationRequest : BudgetOperationRequestBase
    {
        public DateTime? OperationDate { get; set; }
    }
}