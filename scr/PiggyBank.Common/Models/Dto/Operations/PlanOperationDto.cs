using System;
using System.ComponentModel.DataAnnotations;

namespace PiggyBank.Common.Models.Dto.Operations
{
    public class PlanOperationDto : BudgetOperationDto
    {
        [Required]
        public DateTime? PlanDate { get; set; }
    }
}
