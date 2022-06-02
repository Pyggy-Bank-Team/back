using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PiggyBank.Common;
using PiggyBank.Domain.Commands.Accounts;
using PiggyBank.Domain.Results.Accounts;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.Handlers.Accounts
{
    public class ArchiveAccountHandler : IRequestHandler<ArchiveAccountCommand, ArchiveAccountResult>
    {
        private readonly IAccountRepository _repository;

        public ArchiveAccountHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<ArchiveAccountResult> Handle(ArchiveAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetAsync(request.ModifiedBy, request.Id, cancellationToken);

            if (account == null)
                return new ArchiveAccountResult { ErrorCode = ErrorCodes.NotFound };

            if (account.IsArchived || account.IsDeleted)
                return new ArchiveAccountResult { ErrorCode = ErrorCodes.InvalidRequest, Messages = new[] { "Account is already archived or deleted" } };

            account.IsArchived = true;
            account.ModifiedBy = request.ModifiedBy;
            account.ModifiedOn = request.ModifiedOn;

            var _ = await _repository.UpdateAsync(account, cancellationToken);
            return new ArchiveAccountResult();
        }
    }
}
