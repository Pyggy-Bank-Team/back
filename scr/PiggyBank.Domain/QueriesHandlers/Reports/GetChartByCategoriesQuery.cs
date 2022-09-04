using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Reports;
using Common.Results.Models.Dto.Dashboard;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.QueriesHandlers.Reports
{
    public class GetChartByCategoriesQuery : BaseQuery<ChartByCategoryDto[]>
    {
        private readonly GetChartCommand _command;

        public GetChartByCategoriesQuery(PiggyContext context, GetChartCommand command) : base(context)
            => _command = command;

        public override Task<ChartByCategoryDto[]> Invoke(CancellationToken token)
        {
            FixCommandDates(_command);
            var operations = GetRepository<BudgetOperation>().Where(b => !b.IsDeleted  
                                                                         && b.CreatedBy == _command.UserId
                                                                         && b.Category.Type == _command.Type
                                                                         && b.OperationDate >= _command.From
                                                                         && b.OperationDate <= _command.To);

            //TODO: What is it happen when will we use different currencies?
            return operations.Select(o => new
                {
                    o.CategoryId,
                    CategoryTitle = o.Category.Title,
                    CategoryHexColor = o.Category.HexColor,
                    CategoryIsDelete = o.Category.IsDeleted,
                    o.Account.Currency,
                    o.Amount
                }).Where(o => !o.CategoryIsDelete)
                .GroupBy(o => new {o.CategoryId, o.CategoryTitle, o.CategoryHexColor, o.Currency})
                .Select(g => new ChartByCategoryDto
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryTitle = g.Key.CategoryTitle,
                    CategoryHexColor = g.Key.CategoryHexColor,
                    Amount = g.Sum(o => o.Amount),
                    Currency = g.Key.Currency
                }).ToArrayAsync(token);
        }
        
        private void FixCommandDates(GetChartCommand command)
        {
            command.From = command.From.Date;
            command.To = command.To.Date.AddDays(1).AddMilliseconds(-1);
        }
    }
}