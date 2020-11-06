using System;
using PiggyBank.Common.Enums;

namespace PiggyBank.Common.Commands.Reports
{
    public class GetChartCommand
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public CategoryType Type { get; set; }
        public Guid UserId { get; set; }
    }
}