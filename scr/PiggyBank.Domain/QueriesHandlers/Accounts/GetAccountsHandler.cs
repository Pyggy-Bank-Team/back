using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Queries;
using Common.Results.Accounts;
using Common.Results.Models.Dto;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.QueriesHandlers.Accounts
{
    public class GetAccountsHandler : IRequestHandler<GetAccountsQuery, GetAccountsResult>
    {
        private readonly IAccountRepository _repository;

        public GetAccountsHandler(IAccountRepository repository)
            => _repository = repository;

        public Task<GetAccountsResult> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var accounts =  _repository.GetAllAsync(request.UserId);
            var result = new GetAccountsResult
            {
                Data = accounts.Select(a => new AccountDto
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
                }).ToArray()
            };

            return Task.FromResult(result);
        }
    }
}