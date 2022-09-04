using System;
using Common.Commands.Categories;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Categories
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(uc => uc.Id).GreaterThan(0);
            RuleFor(uc => uc.Title).NotEmpty();
            RuleFor(uc => uc.HexColor).NotEmpty();
            RuleFor(uc => uc.ModifiedBy).NotEqual(Guid.Empty);
            RuleFor(uc => uc.ModifiedOn).GreaterThanOrEqualTo(DateTime.UtcNow);
        }
    }
}