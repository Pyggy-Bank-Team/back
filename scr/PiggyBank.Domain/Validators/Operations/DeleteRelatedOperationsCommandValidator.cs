using System;
using Common.Commands.Operations;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Operations
{
    public class DeleteRelatedOperationsCommandValidator : AbstractValidator<DeleteRelatedOperationsCommand>
    {
        public DeleteRelatedOperationsCommandValidator()
        {
            RuleFor(dro => dro.AccountId).GreaterThan(0);
            RuleFor(dro => dro.ModifiedBy).NotEqual(Guid.Empty);
            RuleFor(dro => dro.ModifiedOn).GreaterThanOrEqualTo(DateTime.UtcNow);
        }
    }
}