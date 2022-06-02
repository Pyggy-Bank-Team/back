using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Commands.Accounts;
using Common.Results.Accounts;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.Handlers.Accounts
{
    public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, UpdateAccountResult>
    {
        private readonly IAccountRepository _repository;

        public UpdateAccountHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<UpdateAccountResult> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetAsync(request.ModifiedBy, request.Id, cancellationToken);

            if (account == null)
                return new UpdateAccountResult { ErrorCode = ErrorCodes.NotFound };

            account.Title = request.Title;
            account.Type = request.Type;
            account.Balance = request.Balance;
            account.IsArchived = request.IsArchived;
            account.ModifiedBy = request.ModifiedBy;
            account.ModifiedOn = request.ModifiedOn;

            var _ = await _repository.UpdateAsync(account, cancellationToken);
            return new UpdateAccountResult();
        }
    }
}
