using System;
using System.ComponentModel.DataAnnotations;
using PiggyBank.Common.Interfaces.Models;

namespace PiggyBank.Model.Models
{
    public abstract class EntityBase : IBaseModel
    {
        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        [Key]
        public int Id { get; set; }
    }
}
