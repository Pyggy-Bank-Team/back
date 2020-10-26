using System.ComponentModel.DataAnnotations;
using PiggyBank.Common.Enums;

namespace PiggyBank.WebApi.Requests.Categories
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Title can't be a null or empty")]
        public string Title { get; set; }

        [Required(ErrorMessage = "HexColor can't be a null or empty")]
        public string HexColor { get; set; }

        [Required]
        public CategoryType Type { get; set; }

        public bool IsArchived { get; set; }
    }
}