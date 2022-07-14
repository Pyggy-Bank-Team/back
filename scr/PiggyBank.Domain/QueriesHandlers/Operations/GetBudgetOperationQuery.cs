using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Queries;
using Common.Results.Models.Dto.Operations;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.QueriesHandlers.Operations
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
                    Date = b.OperationDate,
                    AccountId = b.AccountId,
                    CategoryId = b.CategoryId
                })
                .FirstOrDefaultAsync(token);
    }
}