using System;

namespace PiggyBank.Common.Commands
{
    public abstract class BaseCommand
    {
        public Guid UserId { get; set; }
    }
}