using System;
using Newtonsoft.Json;

namespace PiggyBank.Common.Models.Dto.Dashboard
{
    public class ChartByExpensePerDayDto
    {
        [JsonIgnore]
        public int Year { get; set; }
        [JsonIgnore]
        public int Month { get; set; }
        [JsonIgnore]
        public int Day { get; set; }
        public DateTime Date => new DateTime(Year, Month, Day);
        public decimal Amount { get; set; }
    }
}