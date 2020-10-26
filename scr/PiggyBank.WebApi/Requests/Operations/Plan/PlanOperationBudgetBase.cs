using System;
using System.ComponentModel.DataAnnotations;
using PiggyBank.WebApi.Requests.Operations.Budget;

namespace PiggyBank.WebApi.Requests.Operations.Plan
{
    public class PlanOperationBudgetBase : CreateBudgetOperationRequest
    {
        [Required]
        public DateTime PlanDate { get; set; }
    }
}