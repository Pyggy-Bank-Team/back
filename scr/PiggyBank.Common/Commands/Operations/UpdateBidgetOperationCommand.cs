using System;
using System.ComponentModel.DataAnnotations;

namespace PiggyBank.Common.Commands.Operations
{
    public class UpdateBidgetOperationCommand
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int AccountId { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Comment { get; set; }
    }
}
