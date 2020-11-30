using System;
using PiggyBank.Common.Enums;

namespace PiggyBank.Common.Models.Dto
{
    public class OperationDto
    {
        public int Id { get; set; }
        
        public decimal Amount { get; set; }
        
        public OperationType Type { get; set; }
        
        public DateTime Date { get; set; }
        
        public bool IsDeleted { get; set; }

        public OperationAccountDto Account { get; set; }

        public OperationAccountDto ToAccount { get; set; }

        public OperationCategoryDto Category { get; set; }
    }

    public class OperationAccountDto
    {
        public string Title { get; set; }
    }

    public class OperationCategoryDto
    {
        public CategoryType Type { get; set; }
        
        public string HexColor { get; set; }

        public string Title { get; set; }
    }
}
