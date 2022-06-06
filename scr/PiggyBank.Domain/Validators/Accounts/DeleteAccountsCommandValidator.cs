using System;
using Common.Commands.Accounts;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Accounts
{
    public class DeleteAccountsCommandValidator : AbstractValidator<DeleteAccountsCommand>
    {
        public DeleteAccountsCommandValidator()
        {
            RuleFor(da => da.ModifiedBy).NotEqual(Guid.Empty);
            RuleFor(da => da.ModifiedOn).GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleForEach(da => da.Ids).GreaterThan(0);
        }
    }
}