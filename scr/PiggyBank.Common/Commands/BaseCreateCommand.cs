using System;

namespace PiggyBank.Common.Commands
{
    public class BaseCreateCommand : BaseCommand
    {
        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
