using System;

namespace PiggyBank.Common.Commands
{
    public class BaseModifiedCommand : BaseCommand
    {
        public DateTime ModifiedOn { get; set; }
        
        public Guid ModifiedBy { get; set; }
    }
}