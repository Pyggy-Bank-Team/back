using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Reports;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto.Dashboard;
using PiggyBank.Domain.Queries.Reports;

namespace PiggyBank.Domain.Services
{
    public partial class PiggyService : IReportsService
    {
        public Task<ChartByCategoryDto[]> GetChartByCategories(GetChartCommand command, CancellationToken token)
            => _queryDispatcher.Invoke<GetChartByCategoriesQuery, ChartByCategoryDto[]>(token, command);

        public Task<ChartByExpensePerDayDto[]> ChartByExpensePerDays(GetChartCommand command, CancellationToken token)
            => _queryDispatcher.Invoke<GetChartByExpensePerDays, ChartByExpensePerDayDto[]>(token, command);
    }
}