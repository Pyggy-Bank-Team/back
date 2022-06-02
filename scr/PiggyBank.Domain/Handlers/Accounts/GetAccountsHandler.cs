using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Queries;
using PiggyBank.Common.Results.Accounts;
using PiggyBank.Common.Results.Models.Dto;
using PiggyBank.Domain.Queries;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.Handlers.Accounts
{
    public class GetAccountsHandler : IRequestHandler<GetAccountsQuery, GetAccountsResult>
    {
        private readonly IAccountRepository _repository;

        public GetAccountsHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetAccountsResult> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _repository.GetAccountsAsync(request.UserId, cancellationToken);

            if (request.All)
                return new GetAccountsResult
                {
                    Data = await accounts.Select(a => new AccountDto
                    {
                        Id = a.Id,
                        Type = a.Type,
                        Balance = a.Balance,
                        Currency = a.Currency,
                        Title = a.Title,
                        IsArchived = a.IsArchived,
                        IsDeleted = a.IsDeleted,
                        CreatedOn = a.CreatedOn,
                        CreatedBy = a.CreatedBy
                    }).ToArrayAsync(cancellationToken)
                };

            return new GetAccountsResult
            {
                Data = await accounts.Where(a => !a.IsDeleted).Select(a => new AccountDto
                {
                    Id = a.Id,
                    Type = a.Type,
                    Balance = a.Balance,
                    Currency = a.Currency,
                    Title = a.Title,
                    IsArchived = a.IsArchived,
                    IsDeleted = a.IsDeleted,
                    CreatedOn = a.CreatedOn,
                    CreatedBy = a.CreatedBy
                }).ToArrayAsync(cancellationToken)
            };
        }
    }
}