using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Commands.Accounts;
using Common.Results.Accounts;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.CommandHandlers.Accounts
{
    public class ArchiveAccountHandler : IRequestHandler<ArchiveAccountCommand, ArchiveAccountResult>
    {
        private readonly IAccountRepository _repository;

        public ArchiveAccountHandler(IAccountRepository repository)
            =>  _repository = repository;

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

            _ = await _repository.UpdateAsync(account, cancellationToken);
            return new ArchiveAccountResult();
        }
    }
}
