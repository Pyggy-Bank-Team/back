using System;
using Common.Commands.Accounts;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Accounts
{
    public class ArchiveAccountCommandValidator : AbstractValidator<ArchiveAccountCommand>
    {
        public ArchiveAccountCommandValidator()
        {
            RuleFor(aa => aa.Id).GreaterThan(0);
            RuleFor(aa => aa.ModifiedBy).NotEqual(Guid.Empty);
            RuleFor(aa => aa.ModifiedOn).GreaterThanOrEqualTo(DateTime.UtcNow);
        }
    }
}