using System.ComponentModel.DataAnnotations;
using PiggyBank.Common.Enums;

namespace PiggyBank.WebApi.Requests.Categories
{
    public class UpdateCategoryRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string HexColor { get; set; }

        //PB-72
       //public CategoryType Type { get; set; }

        public bool IsArchived { get; set; }
    }
}