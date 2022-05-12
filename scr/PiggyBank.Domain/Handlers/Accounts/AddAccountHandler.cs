using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using PiggyBank.Common;
using PiggyBank.Common.Models.Dto;
using PiggyBank.Domain.Commands.Accounts;
using PiggyBank.Domain.Results.Accounts;
using PiggyBank.Model.Interfaces;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handlers.Accounts
{
    public class AddAccountHandler : IRequestHandler<AddAccountCommand, AddAccountResult>
    {
        private readonly IAccountRepository _repository;
        private readonly IValidator<AddAccountCommand> _validator;

        public AddAccountHandler(IAccountRepository repository, IValidator<AddAccountCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<AddAccountResult> Handle(AddAccountCommand request, CancellationToken cancellationToken)
        {
            var validatorResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
                return new AddAccountResult { ErrorCode = ErrorCodes.InvalidRequest, Messages = validatorResult.Errors.Select(e => e.ErrorMessage).ToArray() };

            var newAccount = new Account
            {
                Balance = request.Balance,
                Currency = request.Currency,
                IsArchived = request.IsArchived,
                Title = request.Title,
                Type = request.Type,
                CreatedBy = request.CreatedBy,
                CreatedOn = request.CreatedOn
            };

            var createdAccount = await _repository.AddAsync(newAccount, cancellationToken);

            return new AddAccountResult
            {
                Data = new AccountDto
                {
                    Id = createdAccount.Id,
                    Balance = createdAccount.Balance,
                    Currency = createdAccount.Currency,
                    Title = createdAccount.Title,
                    Type = createdAccount.Type,
                    IsArchived = createdAccount.IsArchived,
                    CreatedBy = createdAccount.CreatedBy,
                    CreatedOn = createdAccount.CreatedOn,
                    IsDeleted = createdAccount.IsDeleted
                }
            };
        }
    }
}
