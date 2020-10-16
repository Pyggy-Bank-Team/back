using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Dashboard;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto.Dashboard;
using PiggyBank.Domain.Queries.Dashboard;

namespace PiggyBank.Domain.Services
{
    public partial class PiggyService : IDashboardService
    {
        public Task<ChartByCategoryDto[]> GetChartByCategories(GetChartCommand command, CancellationToken token)
            => _queryDispatcher.Invoke<GetChartByCategoriesQuery, ChartByCategoryDto[]>(token, command);
    }
}