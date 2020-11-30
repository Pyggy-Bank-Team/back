using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Models.Dto.Operations;
using PiggyBank.Common.Queries;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Queries.Operations
{
    public class GetBudgetOperationQuery : BaseQuery<BudgetDto>
    {
        private readonly GetOperationQuery _query;

        public GetBudgetOperationQuery(PiggyContext context, GetOperationQuery query) : base(context)
            => _query = query;

        public override Task<BudgetDto> Invoke(CancellationToken token)
            => GetRepository<BudgetOperation>().Where(b => b.CreatedBy == _query.UserId 
                                                           && b.Id == _query.OperationId 
                                                           && !b.IsDeleted)
                .Select(b => new BudgetDto
                {
                    Id = b.Id,
                    Type = b.Type,
                    Comment = b.Comment,
                    Amount = b.Amount,
                    Date = b.CreatedOn,
                    AccountId = b.AccountId,
                    CategoryId = b.CategoryId
                })
                .FirstOrDefaultAsync(token);
    }
}