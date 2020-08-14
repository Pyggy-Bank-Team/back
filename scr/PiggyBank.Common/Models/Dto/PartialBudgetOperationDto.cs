using System.ComponentModel.DataAnnotations;

namespace PiggyBank.Common.Models.Dto
{
    public class PartialBudgetOperationDto
    {
        [Range(1, int.MaxValue)] 
        public int? AccountId { get; set; }

        [Range(1, int.MaxValue)] 
        public int? CategoryId { get; set; }

        public decimal? Amount { get; set; }

        public string? Comment { get; set; }
    }
}