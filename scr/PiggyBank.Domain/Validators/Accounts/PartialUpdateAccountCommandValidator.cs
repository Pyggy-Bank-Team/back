using System;
using Common.Commands.Accounts;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Accounts
{
    public class PartialUpdateAccountCommandValidator : AbstractValidator<PartialUpdateAccountCommand>
    {
        public PartialUpdateAccountCommandValidator()
        {
            RuleFor(puc => puc.Id).GreaterThan(0);
            RuleFor(puc => puc.ModifiedBy).NotEqual(Guid.Empty);
            RuleFor(puc => puc.ModifiedOn).GreaterThanOrEqualTo(DateTime.UtcNow);
        }
    }
}