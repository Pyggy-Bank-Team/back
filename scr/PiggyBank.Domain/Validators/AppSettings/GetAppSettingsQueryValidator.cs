using System;
using Common.Queries;
using FluentValidation;

namespace PiggyBank.Domain.Validators.AppSettings
{
    public class GetAppSettingsQueryValidator : AbstractValidator<GetAppSettingsQuery>
    {
        public GetAppSettingsQueryValidator()
        {
            RuleFor(g => g.UserId).NotEqual(Guid.Empty);
        }
    }
}