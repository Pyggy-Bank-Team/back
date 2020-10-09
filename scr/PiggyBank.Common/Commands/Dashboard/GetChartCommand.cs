using System;

namespace PiggyBank.Common.Commands.Dashboard
{
    public class GetChartCommand
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Guid UserId { get; set; }
    }
}