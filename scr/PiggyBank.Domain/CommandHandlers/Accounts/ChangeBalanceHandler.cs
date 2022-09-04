using System;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Commands.Accounts;
using Common.Results.Accounts;
using MediatR;
using PiggyBank.Model.Interfaces;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.CommandHandlers.Accounts
{
    public class ChangeBalanceHandler : IRequestHandler<ChangeBalanceCommand, ChangeBalanceResult>
    {
        private readonly IAccountRepository _repository;

        public ChangeBalanceHandler(IAccountRepository repository)
            => _repository = repository;

        public async Task<ChangeBalanceResult> Handle(ChangeBalanceCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetAsync(request.UserId, request.AccountId, cancellationToken);

            if (account == null)
                return new ChangeBalanceResult { ErrorCode = ErrorCodes.NotFound};

            account.Balance += request.Amount;
            account.BalanceHistories.Add(new BalanceHistory
            {
                Value = account.Balance,
                CreatedBy = account.CreatedBy,
                CreatedOn = DateTime.UtcNow
            });

            _ = await _repository.UpdateAsync(account, cancellationToken);
            return new ChangeBalanceResult();
        }
    }
}