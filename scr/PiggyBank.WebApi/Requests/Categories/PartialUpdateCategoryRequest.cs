using PiggyBank.Common.Enums;

namespace PiggyBank.WebApi.Requests.Categories
{
    public class PartialUpdateCategoryRequest
    {
        public string Title { get; set; }

        public string HexColor { get; set; }

        public CategoryType? Type { get; set; }
        
        public bool? IsArchived { get; set; }
    }
}