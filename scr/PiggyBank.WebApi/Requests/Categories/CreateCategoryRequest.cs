using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace PiggyBank.WebApi.Requests.Categories
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string HexColor { get; set; }

        [Required]
        public CategoryType Type { get; set; }

        public bool IsArchived { get; set; }
    }
}