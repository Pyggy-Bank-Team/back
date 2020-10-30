using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Models.Dto;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Enums;
using PiggyBank.Common.Models;

namespace PiggyBank.Domain.Queries.Operations
{
    public class GetOperationsQuery : BaseQuery<PageResult<OperationDto>>
    {
        private readonly Guid _userId;
        private readonly int _page;

        public GetOperationsQuery(PiggyContext context, Guid userId, int page)
            : base(context)
            => (_userId, _page) = (userId, page);

        public override async Task<PageResult<OperationDto>> Invoke(CancellationToken token)
        {
            var budgetQuery = GetRepository<BudgetOperation>().Where(b => b.CreatedBy == _userId && !b.IsDeleted && b.Type == OperationType.Budget)
                .Select(b => new OperationDto
                {
                    Id = b.Id,
                    CategoryId = b.CategoryId,
                    CategoryType = b.Category.Type,
                    CategoryHexColor = b.Category.HexColor,
                    Amount = b.Amount,
                    AccountId = b.AccountId,
                    AccountTitle = b.Account.Title,
                    Comment = b.Comment,
                    Type = b.Type,
                    CreatedOn = b.CreatedOn,
                    PlanDate = null,
                    FromTitle = null,
                    ToTitle = null,
                    IsDeleted = b.IsDeleted
                });

            var transferQuery = GetRepository<TransferOperation>().Where(t => t.CreatedBy == _userId && !t.IsDeleted)
                .Select(t => new OperationDto
                {
                    Id = t.Id,
                    CategoryId = 0,
                    CategoryType = null,
                    CategoryHexColor = null,
                    Amount = t.Amount,
                    AccountId = 0,
                    AccountTitle = null,
                    Comment = t.Comment,
                    Type = t.Type,
                    CreatedOn = t.CreatedOn,
                    PlanDate = null,
                    FromTitle = GetRepository<Account>().First(a => a.Id == t.From).Title,
                    ToTitle = GetRepository<Account>().First(a => a.Id == t.To).Title,
                    IsDeleted = t.IsDeleted
                });

            var planQuery = GetRepository<PlanOperation>().Where(p => p.CreatedBy == _userId && !p.IsDeleted)
                .Select(p => new OperationDto
                {
                    Id = p.Id,
                    CategoryId = 0,
                    CategoryType = p.Category.Type,
                    CategoryHexColor = p.Category.HexColor,
                    Amount = p.Amount,
                    AccountId = 0,
                    AccountTitle = p.Account.Title,
                    Comment = p.Comment,
                    Type = p.Type,
                    CreatedOn = p.CreatedOn,
                    PlanDate = p.PlanDate,
                    FromTitle = null,
                    ToTitle = null,
                    IsDeleted = p.IsDeleted
                });

            var operationsQuery = budgetQuery.Union(transferQuery).Union(planQuery).OrderByDescending(o => o.CreatedOn);

            var result = new PageResult<OperationDto>
            {
                CurrentPage = _page
            };

            var totalCount = await operationsQuery.CountAsync(token);

            result.TotalPages = totalCount / result.CountItemsOnPage + (totalCount % result.CountItemsOnPage > 0 ? 1 : 0);
            result.Result = await operationsQuery.Skip(result.CountItemsOnPage * (result.CurrentPage - 1)).Take(result.CountItemsOnPage)
                .ToArrayAsync(token);

            return result;
        }
    }
}