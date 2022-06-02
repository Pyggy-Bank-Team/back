using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PiggyBank.Common;
using PiggyBank.Common.Queries;
using PiggyBank.Common.Results.Accounts;
using PiggyBank.Common.Results.Models.Dto;
using PiggyBank.Domain.Queries;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.Handlers.Accounts
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountQuery, GetAccountResult>
    {
        private readonly IAccountRepository _repository;

        public GetAccountByIdHandler(IAccountRepository repository)
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