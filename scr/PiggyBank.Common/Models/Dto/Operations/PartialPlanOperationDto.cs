using System;

namespace PiggyBank.Common.Models.Dto.Operations
{
    public class PartialPlanOperationDto : PartialBudgetOperationDto
    {
        public DateTime? PlanDate { get; set; }
    }
}