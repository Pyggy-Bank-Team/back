using System;

namespace PiggyBank.WebApi.Requests.Dashboard
{
    public class GetChartByCategoriesRequest
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}