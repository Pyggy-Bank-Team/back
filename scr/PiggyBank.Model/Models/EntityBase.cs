using System;
using System.ComponentModel.DataAnnotations;

namespace PiggyBank.Model.Models
{
    public abstract class EntityBase
    {
        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        [Key]
        public int Id { get; set; }
    }
}
