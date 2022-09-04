using Common.Enums;

namespace Common.Results.Models.Dto.Operations
{
    public class OperationDto : OperationBase
    {
        public bool IsDeleted { get; set; }

        public OperationAccountDto Account { get; set; }

        public OperationAccountDto ToAccount { get; set; }

        public OperationCategoryDto Category { get; set; }
    }

    public class OperationAccountDto
    {
        public string Title { get; set; }
        
        public string Currency { get; set; }
    }

    public class OperationCategoryDto
    {
        public CategoryType Type { get; set; }
        
        public string HexColor { get; set; }

        public string Title { get; set; }
    }
}
