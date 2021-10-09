using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Reports;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto.Dashboard;
using PiggyBank.Domain.Queries.Reports;
using PiggyBank.Model;
using Serilog;

namespace PiggyBank.Domain.Services
{
    public class ReportService : PiggyServiceBase, IReportService
    {
        public ReportService(PiggyContext context, ILogger logger) : base(context, logger)
        {
        }
        
        public Task<ChartByCategoryDto[]> GetChartByCategories(GetChartCommand command, CancellationToken token)
            => QueryDispatcher.Invoke<GetChartByCategoriesQuery, ChartByCategoryDto[]>(token, command);

        public Task<ChartByExpensePerDayDto[]> ChartByExpensePerDays(GetChartCommand command, CancellationToken token)
            => QueryDispatcher.Invoke<GetChartByExpensePerDays, ChartByExpensePerDayDto[]>(token, command);
    }
}