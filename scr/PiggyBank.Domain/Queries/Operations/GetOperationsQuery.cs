using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Models.Dto;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Operations;
using PiggyBank.Common.Enums;
using PiggyBank.Common.Models;
using PiggyBank.Common.Models.Dto.Operations;

namespace PiggyBank.Domain.Queries.Operations
{
    public class GetOperationsQuery : BaseQuery<PageResult<OperationDto>>
    {
        private readonly GetOperationsCommand _command;

        public GetOperationsQuery(PiggyContext context, GetOperationsCommand command)
            : base(context)
            => _command = command;

        public override async Task<PageResult<OperationDto>> Invoke(CancellationToken token)
        {
            var budgetQuery = _command.WithDeleted
                ? GetRepository<BudgetOperation>().Where(b => b.CreatedBy == _command.UserId && b.Type == OperationType.Budget)
                : GetRepository<BudgetOperation>().Where(b => b.CreatedBy == _command.UserId
                                                              && !b.IsDeleted
                                                              && b.Type == OperationType.Budget);

            var budgets = budgetQuery.Select(b =>
                new
                {
                    b.Id,
                    b.Amount,
                    b.Type,
                    Date = b.CreatedOn,
                    b.IsDeleted,
                    AccountTitle = b.Account.Title,
                    ToAccountTitle = "",
                    CategoryTitle = b.Category.Title,
                    CategoryHexColor = b.Category.HexColor,
                    CategoryType = b.Category.Type
                });

            var transferQuery = _command.WithDeleted
                ? GetRepository<TransferOperation>().Where(t => t.CreatedBy == _command.UserId)
                : GetRepository<TransferOperation>().Where(t => t.CreatedBy == _command.UserId && !t.IsDeleted);

            var transfers = transferQuery.Select(t =>
                new
                {
                    t.Id,
                    t.Amount,
                    t.Type,
                    Date = t.CreatedOn,
                    t.IsDeleted,
                    AccountTitle = GetRepository<Account>().First(a => a.Id == t.From).Title,
                    ToAccountTitle = GetRepository<Account>().First(a => a.Id == t.To).Title,
                    CategoryTitle = "",
                    CategoryHexColor = "",
                    CategoryType = CategoryType.Undefined
                });

            var operationsQuery = budgets.Union(transfers).Select(o => new OperationDto
            {
                Id = o.Id,
                Amount = o.Amount,
                Type = o.Type,
                Date = o.Date,
                IsDeleted = o.IsDeleted,
                Account = new OperationAccountDto
                {
                    Title = o.AccountTitle
                },
                ToAccount = o.Type == OperationType.Budget ? null : new OperationAccountDto {Title = o.ToAccountTitle},
                Category = o.Type == OperationType.Transfer
                    ? null
                    : new OperationCategoryDto
                    {
                        Title = o.CategoryTitle,
                        HexColor = o.CategoryHexColor,
                        Type = o.CategoryType
                    }
            }).OrderByDescending(o => o.Date);

            var result = new PageResult<OperationDto>
            {
                CurrentPage = _command.Page
            };

            var totalCount = await operationsQuery.CountAsync(token);

            result.TotalPages = totalCount / result.CountItemsOnPage + (totalCount % result.CountItemsOnPage > 0 ? 1 : 0);
            result.Result = await operationsQuery.Skip(result.CountItemsOnPage * (result.CurrentPage - 1)).Take(result.CountItemsOnPage)
                .ToArrayAsync(token);

            return result;
        }
    }
}