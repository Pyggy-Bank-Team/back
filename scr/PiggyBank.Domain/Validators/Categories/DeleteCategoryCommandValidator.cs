using System;
using Common.Commands.Categories;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Categories
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(d => d.Id).GreaterThan(0);
            RuleFor(d => d.Locale).NotEmpty();
            RuleFor(d => d.ModifiedBy).NotEqual(Guid.Empty);
            RuleFor(d => d.ModifiedOn).GreaterThanOrEqualTo(DateTime.UtcNow);
        }
    }
}