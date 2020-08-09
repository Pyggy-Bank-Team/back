using PiggyBank.Common.Enums;

namespace PiggyBank.Common.Models.Dto
{
    public class PartialCategoryDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string HexColor { get; set; }

        public CategoryType? Type { get; set; }
    }
}
