using System;
using Common.Enums;

namespace Common.Commands.Reports
{
    public class GetChartCommand
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public CategoryType Type { get; set; }
        public Guid UserId { get; set; }
    }
}