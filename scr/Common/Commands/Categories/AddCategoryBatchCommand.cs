using Common.Enums;

namespace Common.Commands.Categories
{
    public class AddCategoryBatchCommand : BaseCreateCommand
    {
        public CategoryItem[] Categories { get; set; }
    }

    public class CategoryItem
    {
        public string Title { get; set; }

        public string HexColor { get; set; }

        public CategoryType Type { get; set; }

        public bool IsArchived { get; set; }
    }
}