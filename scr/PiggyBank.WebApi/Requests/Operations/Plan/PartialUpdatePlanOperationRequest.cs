using System;
using PiggyBank.WebApi.Requests.Operations.Budget;

namespace PiggyBank.WebApi.Requests.Operations.Plan
{
    public class PartialUpdatePlanOperationRequest : PartialUpdateBudgetOperationRequest
    {
        public DateTime? PlanDate { get; set; }
    }
}