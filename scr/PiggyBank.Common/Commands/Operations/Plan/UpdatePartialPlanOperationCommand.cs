using System;
using PiggyBank.Common.Commands.Operations.Budget;

namespace PiggyBank.Common.Commands.Operations.Plan
{
    public class UpdatePartialPlanOperationCommand : UpdatePartialBidgetOperationCommand
    {
        public DateTime? PlanDate { get; set; }
    }
}