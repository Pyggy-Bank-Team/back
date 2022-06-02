using System.ComponentModel.DataAnnotations;
using PiggyBank.Common.Enums;

namespace PiggyBank.Model.Models.Entities
{
    public class Category : DeletedEntityBase
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string HexColor { get; set; }

        public CategoryType Type { get; set; }
    }
}
