using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace Common.Results.Models.Dto
{
    public class CategoryDto : BaseDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Title can't be a null or empty")]
        public string Title { get; set; }

        [Required(ErrorMessage = "HexColor can't be a null or empty")]
        public string HexColor { get; set; }

        public CategoryType Type { get; set; }
        
        public bool IsDeleted { get; set; }

        public bool IsArchived { get; set; }
    }
}
