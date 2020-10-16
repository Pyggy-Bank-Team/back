using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Dashboard;
using PiggyBank.Common.Models.Dto.Dashboard;

namespace PiggyBank.Common.Interfaces
{
    public interface IDashboardService
    {
        Task<ChartByCategoryDto[]> GetChartByCategories(GetChartCommand command, CancellationToken token);
        Task<ChartByExpensePerDayDto[]> ChartByExpensePerDays(GetChartCommand command, CancellationToken token);
    }
}