using System.ComponentModel.DataAnnotations;
using PiggyBank.Common.Enums;

namespace PiggyBank.Model.Models.Entities
{
    public class BotOperation : EntityModifiedBase
    {
        [Required]
        public long ChatId { get; set; }

        [Required] 
        public OperationType Type { get; set; }
        
        [Required]
        public CreationStage Stage { get; set; }
        
        public CategoryType? CategoryType { get; set; }
        
        public decimal? Amount { get; set; }
        
        public int? AccountId { get; set; }
        
        public int? CategoryId { get; set; }
        
        public int? ToAccountId { get; set; }
    }
}