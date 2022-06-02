using Common.Commands.Accounts;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Accounts
{
    public class AddAccountCommandValidator : AbstractValidator<AddAccountCommand>
    {
        public AddAccountCommandValidator()
        {
            
        }
    }
}