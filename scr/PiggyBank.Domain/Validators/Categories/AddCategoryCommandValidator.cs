using System;
using Common.Commands.Categories;
using Common.Enums;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Categories
{
    public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
    {
        public AddCategoryCommandValidator()
        {
            RuleFor(c => c.Type).NotEqual(CategoryType.Undefined);
            RuleFor(c => c.Title).NotEmpty();
            RuleFor(c => c.HexColor).NotEmpty();
            RuleFor(c => c.CreatedBy).NotEqual(Guid.Empty);
            RuleFor(c => c.CreatedOn).ExclusiveBetween(DateTime.UtcNow.AddMinutes(-2), DateTime.UtcNow.AddMinutes(2));
        }
    }
}