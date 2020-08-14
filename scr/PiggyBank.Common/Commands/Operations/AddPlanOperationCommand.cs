using System;
using PiggyBank.Common.Commands.Operations.Budget;

namespace PiggyBank.Common.Commands.Operations
{
    public class AddPlanOperationCommand : AddBudgetOperationCommand
    {
        public DateTime PlanDate { get; set; }
    }
}
