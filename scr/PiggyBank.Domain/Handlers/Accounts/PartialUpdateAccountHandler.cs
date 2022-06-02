using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PiggyBank.Common;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Common.Results.Accounts;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.Handlers.Accounts
{
    public class PartialUpdateAccountHandler : IRequestHandler<PartialUpdateAccountCommand, PartialUpdateAccountResult>
    {
        private readonly IAccountRepository _repository;

        public PartialUpdateAccountHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<PartialUpdateAccountResult> Handle(PartialUpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetAsync(request.ModifiedBy, request.Id, cancellationToken);

            if (account == null || account.IsDeleted)
                return new PartialUpdateAccountResult { ErrorCode = ErrorCodes.InvalidRequest, Messages = new[] { "Account not found or deleted" } };

            account.Title = string.IsNullOrWhiteSpace(request.Title) ? account.Title : request.Title;
            account.Type = request.Type ?? account.Type;
            account.Balance = request.Balance ?? account.Balance;
            account.IsArchived = request.IsArchive ?? account.IsArchived;
            account.ModifiedBy = request.ModifiedBy;
            account.ModifiedOn = request.ModifiedOn;

            var _ = await _repository.UpdateAsync(account, cancellationToken);
            return new PartialUpdateAccountResult();
        }
    }
}