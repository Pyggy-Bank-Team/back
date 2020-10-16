using System;

namespace PiggyBank.WebApi.Requests.Dashboard
{
    public class GetChartRequest
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}