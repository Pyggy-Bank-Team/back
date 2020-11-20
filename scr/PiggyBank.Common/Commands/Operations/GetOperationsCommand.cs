using System;

namespace PiggyBank.Common.Commands.Operations
{
    public class GetOperationsCommand
    {
        public Guid UserId { get; set; }
        public int Page { get; set; }
        public bool WithDeleted { get; set; }
    }
}