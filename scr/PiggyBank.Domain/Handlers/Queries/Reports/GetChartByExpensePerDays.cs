using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Reports;
using Common.Enums;
using Common.Results.Models.Dto.Dashboard;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Queries.Reports
{
    public class GetChartByExpensePerDays : BaseQuery<ChartByExpensePerDayDto[]>
    {
        private readonly GetChartCommand _command;

        public GetChartByExpensePerDays(PiggyContext context, GetChartCommand command) : base(context)
            => _command = command;

        public override Task<ChartByExpensePerDayDto[]> Invoke(CancellationToken token)
        {
            FixCommandDates(_command);
            var operations = GetRepository<BudgetOperation>().Where(b => !b.IsDeleted
                                                                         && b.CreatedBy == _command.UserId
                                                                         && b.Category.Type == CategoryType.Expense
                                                                         && b.OperationDate >= _command.From
                                                                         && b.OperationDate <=  _command.To);

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

        private void FixCommandDates(GetChartCommand command)
        {
            command.From = command.From.Date;
            command.To = command.To.Date.AddDays(1).AddMilliseconds(-1);
        }
    }
}