using FluentValidation;
using PiggyBank.Common.Commands.Accounts;

namespace PiggyBank.Domain.Validators.Accounts
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator()
        {
            RuleFor(uc => uc.Id).GreaterThan(0);
            RuleFor(uc => uc.Title).NotEmpty().NotNull();
            RuleFor(uc => uc.Type).IsInEnum();
            RuleFor(uc => uc.Currency).NotEmpty().NotNull();
        }
    }
}