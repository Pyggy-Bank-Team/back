using Common.Enums;

namespace Common.Commands.Categories
{
    public class AddCategoryCommand : BaseCreateCommand
    {
        public string Title { get; set; }

        public string HexColor { get; set; }

        public CategoryType Type { get; set; }

        public bool IsArchived { get; set; }
    }
}
