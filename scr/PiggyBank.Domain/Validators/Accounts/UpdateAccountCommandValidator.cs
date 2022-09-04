using System;
using Common.Commands.Accounts;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Accounts
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator()
        {
            RuleFor(uc => uc.Id).GreaterThan(0);
            RuleFor(uc => uc.Title).NotEmpty();
            RuleFor(uc => uc.Type).IsInEnum();
            RuleFor(uc => uc.Currency).NotEmpty();
            RuleFor(uc => uc.ModifiedBy).NotEqual(Guid.Empty);
            RuleFor(uc => uc.ModifiedOn).GreaterThanOrEqualTo(DateTime.UtcNow);
        }
    }
}