using System;
using Common.Commands.Accounts;
using Common.Enums;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Accounts
{
    public class AddAccountCommandValidator : AbstractValidator<AddAccountCommand>
    {
        public AddAccountCommandValidator()
        {
            RuleFor(a => a.Type).IsInEnum().NotEqual(AccountType.Undefined);
            RuleFor(a => a.Currency).NotEmpty();
            RuleFor(a => a.CreatedBy).NotEqual(Guid.Empty);
            RuleFor(a => a.Title).NotEmpty();
            RuleFor(a => a.CreatedOn).ExclusiveBetween(DateTime.UtcNow.AddMinutes(-2), DateTime.UtcNow.AddMinutes(2));
        }
    }
}