using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Common.Enums;
using PiggyBank.Common.Results.Accounts;

namespace PiggyBank.Domain.Handlers.Accounts
{
    public class AddDefaultAccountsHandler : IRequestHandler<AddDefaultAccountsCommand, AddDefaultAccountsResult>
    {
        private readonly IMediator _mediator;

        public AddDefaultAccountsHandler(IMediator mediator)
            => _mediator = mediator;

        public async Task<AddDefaultAccountsResult> Handle(AddDefaultAccountsCommand request, CancellationToken cancellationToken)
        {
            if (IsRussianLanguage(request.Locale))
            {
                foreach (var command in GenerateAccountsWithRussianTitles(request))
                {
                    _ = await _mediator.Send(command, cancellationToken);
                }
            }
            else
            {
                foreach (var command in GenerateAccountsWithEnglishTitles(request))
                {
                    _ = await _mediator.Send(command, cancellationToken);
                }
            }

            return new AddDefaultAccountsResult();
        }

        private bool IsRussianLanguage(string locale)
        {
            if (string.IsNullOrWhiteSpace(locale))
                return false;

            var lower = locale.ToLowerInvariant();
            return lower.Contains("ru") || lower.Contains("kz") || lower.Contains("by") || lower.Contains("ua");
        }

        private IEnumerable<AddAccountCommand> GenerateAccountsWithRussianTitles(AddDefaultAccountsCommand request)
        {
            yield return new AddAccountCommand
            {
                Balance = 100M,
                Currency = request.Currency,
                Title = "Наличные",
                IsArchived = false,
                Type = AccountType.Cash,
                CreatedBy = request.CreatedBy,
                CreatedOn = request.CreatedOn
            };

            yield return new AddAccountCommand
            {
                Balance = 100M,
                Currency = request.Currency,
                Title = "Дебетовая карточка",
                IsArchived = false,
                Type = AccountType.Card,
                CreatedBy = request.CreatedBy,
                CreatedOn = request.CreatedOn
            };
        }

        private IEnumerable<AddAccountCommand> GenerateAccountsWithEnglishTitles(AddDefaultAccountsCommand request)
        {
            yield return new AddAccountCommand
            {
                Balance = 100M,
                Currency = request.Currency,
                Title = "Cash",
                IsArchived = false,
                Type = AccountType.Cash,
                CreatedBy = request.CreatedBy,
                CreatedOn = request.CreatedOn
            };

            yield return new AddAccountCommand
            {
                Balance = 100M,
                Currency = request.Currency,
                Title = "Card",
                IsArchived = false,
                Type = AccountType.Card,
                CreatedBy = request.CreatedBy,
                CreatedOn = request.CreatedOn
            };
        }
    }
}