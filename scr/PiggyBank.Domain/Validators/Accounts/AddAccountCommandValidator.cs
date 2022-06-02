using FluentValidation;
using PiggyBank.Common.Commands.Accounts;

namespace PiggyBank.Domain.Validators.Accounts
{
    public class AddAccountCommandValidator : AbstractValidator<AddAccountCommand>
    {
        public AddAccountCommandValidator()
        {
            
        }
    }
}