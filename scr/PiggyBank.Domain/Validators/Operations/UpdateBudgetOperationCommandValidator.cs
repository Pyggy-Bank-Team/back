using System;
using Common.Commands.Operations.Budget;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Operations
{
    public class UpdateBudgetOperationCommandValidator : AbstractValidator<UpdateBudgetOperationCommand>
    {
        public UpdateBudgetOperationCommandValidator()
        {
            RuleFor(ub => ub.Amount).NotEqual(decimal.MinValue);
            RuleFor(ub => ub.Id).GreaterThan(0);
            RuleFor(ub => ub.AccountId).GreaterThan(0);
            RuleFor(ub => ub.CategoryId).GreaterThan(0);
            RuleFor(ub => ub.ModifiedBy).NotEqual(Guid.Empty);
            RuleFor(ub => ub.ModifiedOn).ExclusiveBetween(DateTime.UtcNow.AddMinutes(-2), DateTime.UtcNow.AddMinutes(2));
        }
    }
}