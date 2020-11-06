using System;

namespace PiggyBank.WebApi.Requests.Reports
{
    public abstract class ChartRequestBase
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}