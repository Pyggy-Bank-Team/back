using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Queries;
using Common.Results;
using Common.Results.Models.Dto.Operations;
using MediatR;
using PiggyBank.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PiggyBank.Domain.QueriesHandlers.Operations
{
    public class GetOperationsHandler : IRequestHandler<GetOperationsQuery, PageResult<OperationDto>>
    {
        private readonly ITransferOperationRepository _transferOperationRepository;
        private readonly IBudgetOperationRepository _budgetOperationRepository;

        public GetOperationsHandler(ITransferOperationRepository transferOperationRepository, IBudgetOperationRepository budgetOperationRepository)
        {
            _transferOperationRepository = transferOperationRepository;
            _budgetOperationRepository = budgetOperationRepository;
        }

        public async Task<PageResult<OperationDto>> Handle(GetOperationsQuery request, CancellationToken cancellationToken)
        {
            var budgetOperations = _budgetOperationRepository.GetAllAsQueryable(request.UserId);
            var transferOperations = _transferOperationRepository.GetAllAsQueryable(request.UserId);

            var operationsQuery = budgetOperations.Union(transferOperations).Select(o => new OperationDto
            {
                Id = o.Id,
                Amount = o.Amount,
                Type = o.Type,
                Date = o.Date,
                IsDeleted = o.IsDeleted,
                Comment = o.Comment,
                Account = new OperationAccountDto
                {
                    Title = o.AccountTitle,
                    Currency = o.AccountCurrency
                },
                ToAccount = o.Type == OperationType.Budget ? null : new OperationAccountDto {Title = o.ToAccountTitle, Currency = o.ToAccountCurrency},
                Category = o.Type == OperationType.Transfer ? null : new OperationCategoryDto { Title = o.CategoryTitle, HexColor = o.CategoryHexColor, Type = o.CategoryType }
            }).OrderByDescending(o => o.Date);

            var result = new PageResult<OperationDto>
            {
                CurrentPage = request.Page
            };

            var totalCount = await operationsQuery.CountAsync(cancellationToken);

            result.TotalPages = totalCount / result.CountItemsOnPage + (totalCount % result.CountItemsOnPage > 0 ? 1 : 0);
            result.Result = await operationsQuery.Skip(result.CountItemsOnPage * (result.CurrentPage - 1)).Take(result.CountItemsOnPage).ToArrayAsync(token);

            return result;
        }
    }
}