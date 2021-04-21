using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Reports;
using PiggyBank.Common.Models.Dto.Dashboard;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Queries.Reports
{
    public class GetChartByCategoriesQuery : BaseQuery<ChartByCategoryDto[]>
    {
        private readonly GetChartCommand _command;

        public GetChartByCategoriesQuery(PiggyContext context, GetChartCommand command) : base(context)
            => _command = command;

        public override Task<ChartByCategoryDto[]> Invoke(CancellationToken token)
        {
            var operations = GetRepository<BudgetOperation>().Where(b => !b.IsDeleted  
                                                                         && b.CreatedBy == _command.UserId
                                                                         && b.Category.Type == _command.Type
                                                                         && b.CreatedOn >= _command.From
                                                                         && b.CreatedOn <= _command.To);

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
    }
}