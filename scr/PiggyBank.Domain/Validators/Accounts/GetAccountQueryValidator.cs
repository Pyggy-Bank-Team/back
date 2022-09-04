using System;
using Common.Queries;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Accounts
{
    public class GetAccountQueryValidator : AbstractValidator<GetAccountQuery>
    {
        public GetAccountQueryValidator()
        {
            RuleFor(ga => ga.AccountId).GreaterThan(0);
            RuleFor(ga => ga.UserId).NotEqual(Guid.Empty);
        }
    }
}