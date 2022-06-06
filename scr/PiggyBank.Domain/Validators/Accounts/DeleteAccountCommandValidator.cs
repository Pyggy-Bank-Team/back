using System;
using Common.Commands.Accounts;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Accounts
{
    public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
    {
        public DeleteAccountCommandValidator()
        {
            RuleFor(da => da.Id).GreaterThan(0);
            RuleFor(da => da.ModifiedBy).NotEqual(Guid.Empty);
            RuleFor(da => da.ModifiedOn).GreaterThanOrEqualTo(DateTime.UtcNow);
        }
    }
}