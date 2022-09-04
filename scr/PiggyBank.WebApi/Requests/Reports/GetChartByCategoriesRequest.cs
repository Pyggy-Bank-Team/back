using Common.Enums;

namespace PiggyBank.WebApi.Requests.Reports
{
    public class GetChartByCategoriesRequest : ChartRequestBase
    {
        public CategoryType Type { get; set; }
    }
}