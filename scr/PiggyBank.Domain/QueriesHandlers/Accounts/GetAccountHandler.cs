using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Queries;
using Common.Results.Accounts;
using Common.Results.Models.Dto;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.QueriesHandlers.Accounts
{
    public class GetAccountHandler : IRequestHandler<GetAccountQuery, GetAccountResult>
    {
        private readonly IAccountRepository _repository;

        public GetAccountHandler(IAccountRepository repository)
            => _repository = repository;

        public async Task<GetAccountResult> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetAsync(request.UserId, request.AccountId, cancellationToken);

            if (account == null)
                return new GetAccountResult { ErrorCode = ErrorCodes.NotFound };

            return new GetAccountResult
            {
                Data = new AccountDto
                {
                    Id = account.Id,
                    Balance = account.Balance,
                    Currency = account.Currency,
                    IsArchived = account.IsArchived,
                    IsDeleted = account.IsDeleted,
                    Title = account.Title,
                    Type = account.Type,
                    CreatedOn = account.CreatedOn,
                    CreatedBy = account.CreatedBy
                }
            };
        }
    }
}