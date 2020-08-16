using System;
using PiggyBank.Common.Commands.Operations.Budget;

namespace PiggyBank.Common.Commands.Operations.Plan
{
    public class UpdatePlanOperationCommand : UpdateBidgetOperationCommand
    {
        public DateTime PlanDate { get; set; }
    }
}