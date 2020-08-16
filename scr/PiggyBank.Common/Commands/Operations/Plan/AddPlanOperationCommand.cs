using System;
using PiggyBank.Common.Commands.Operations.Budget;

namespace PiggyBank.Common.Commands.Operations.Plan
{
    public class AddPlanOperationCommand : AddBudgetOperationCommand
    {
        public DateTime PlanDate { get; set; }
    }
}
