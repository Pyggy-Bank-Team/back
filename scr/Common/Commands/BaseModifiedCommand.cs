using System;

namespace Common.Commands
{
    public class BaseModifiedCommand 
    {
        public DateTime ModifiedOn { get; set; }
        
        public Guid ModifiedBy { get; set; }
    }
}