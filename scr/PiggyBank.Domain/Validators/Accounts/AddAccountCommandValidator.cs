using FluentValidation;
using PiggyBank.Domain.Commands.Accounts;

namespace PiggyBank.Domain.Validators.Accounts
{
    public class AddAccountCommandValidator : AbstractValidator<AddAccountCommand>
    {
        public AddAccountCommandValidator()
        {
            
        }
    }
}