using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Dashboard;
using PiggyBank.Common.Enums;
using PiggyBank.Common.Models.Dto.Dashboard;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Queries.Dashboard
{
    public class GetChartByExpensePerDays : BaseQuery<ChartByExpensePerDayDto[]>
    {
        private readonly GetChartCommand _command;

        public GetChartByExpensePerDays(PiggyContext context, GetChartCommand command) : base(context)
            => _command = command;

        public override Task<ChartByExpensePerDayDto[]> Invoke(CancellationToken token)
        {
            var operations = GetRepository<BudgetOperation>().Where(b => b.CreatedBy == _command.UserId
                                                                         && b.Category.Type == CategoryType.Expense
                                                                         && b.CreatedOn >= _command.From
                                                                         && b.CreatedOn <= _command.To);

            return operations.Select(o => new {o.Amount, o.CreatedOn.Year, o.CreatedOn.Month, o.CreatedOn.Day})
                .GroupBy(o => new {o.Year, o.Month, o.Day})
                .Select(g => new ChartByExpensePerDayDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Day = g.Key.Day,
                    Amount = g.Sum(o => o.Amount)
                }).ToArrayAsync(token);
        }
    }
}