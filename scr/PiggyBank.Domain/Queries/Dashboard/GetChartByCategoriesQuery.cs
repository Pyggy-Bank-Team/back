using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Dashboard;
using PiggyBank.Common.Models.Dto.Dashboard;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Queries.Dashboard
{
    public class GetChartByCategoriesQuery : BaseQuery<ChartByCategoryDto[]>
    {
        private readonly GetChartCommand _command;

        public GetChartByCategoriesQuery(PiggyContext context, GetChartCommand command) : base(context)
            => _command = command;

        public override Task<ChartByCategoryDto[]> Invoke(CancellationToken token)
        {
            var operations = GetRepository<BudgetOperation>().Where(b => b.CreatedBy == _command.UserId
                                                                         && b.CreatedOn >= _command.From
                                                                         && b.CreatedOn <= _command.To);
            return operations.Select(o => new
                {
                    o.CategoryId,
                    CategoryTitle = o.Category.Title,
                    CategoryHexColor = o.Category.HexColor,
                    o.Amount
                }).GroupBy(o => new {o.CategoryId, o.CategoryTitle, o.CategoryHexColor})
                .Select(g => new ChartByCategoryDto
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryTitle = g.Key.CategoryTitle,
                    CategoryHexColor = g.Key.CategoryHexColor,
                    Amount = g.Sum(o => o.Amount)
                }).ToArrayAsync(token);
        }
    }
}