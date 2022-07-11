using System;
using Common.Commands.Categories;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Categories
{
    public class AddDefaultCategoriesCommandValidator : AbstractValidator<AddDefaultCategoriesCommand>
    {
        public AddDefaultCategoriesCommandValidator()
        {
            RuleFor(c => c.Locale).NotEmpty();
            RuleFor(c => c.CreatedBy).NotEqual(Guid.Empty);
            RuleFor(c => c.CreatedOn).ExclusiveBetween(DateTime.UtcNow.AddMinutes(-2), DateTime.UtcNow.AddMinutes(2));
        }
    }
}