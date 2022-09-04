using System;
using Common.Commands.Categories;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Categories
{
    public class PartialUpdateCategoryCommandValidator : AbstractValidator<PartialUpdateCategoryCommand>
    {
        public PartialUpdateCategoryCommandValidator()
        {
            RuleFor(pu => pu.Id).GreaterThan(0);
            RuleFor(pu => pu.ModifiedBy).NotEqual(Guid.Empty);
            RuleFor(pu => pu.ModifiedOn).GreaterThanOrEqualTo(DateTime.UtcNow);
        }
    }
}