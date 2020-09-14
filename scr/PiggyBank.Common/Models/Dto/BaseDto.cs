using System;

namespace PiggyBank.Common.Models.Dto
{
    public class BaseDto
    {
        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}