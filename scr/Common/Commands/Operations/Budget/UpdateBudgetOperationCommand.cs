using System;
using System.ComponentModel.DataAnnotations;
using Common.Results.Operations.Budget;
using MediatR;

namespace Common.Commands.Operations.Budget
{
    public class UpdateBudgetOperationCommand : BaseModifiedCommand, IRequest<UpdateBudgetOperationResult>
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int AccountId { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Comment { get; set; }

        public DateTime? OperationDate { get; set; }
    }
}
