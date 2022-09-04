using System;
using Common.Queries;
using FluentValidation;

namespace PiggyBank.Domain.Validators.Accounts
{
    public class GetAccountsQueryValidator : AbstractValidator<GetAccountsQuery>
    {
        public GetAccountsQueryValidator()
        {
            RuleFor(g => g.UserId).NotEqual(Guid.Empty);
        }
    }
}