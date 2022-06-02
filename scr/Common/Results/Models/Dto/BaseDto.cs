using System;

namespace Common.Results.Models.Dto
{
    public class BaseDto
    {
        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}