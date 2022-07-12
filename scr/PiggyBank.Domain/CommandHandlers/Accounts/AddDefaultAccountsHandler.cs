using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Accounts;
using Common.Enums;
using Common.Results.Accounts;
using MediatR;
using PiggyBank.Domain.Helpers;

namespace PiggyBank.Domain.CommandHandlers.Accounts
{
    public class AddDefaultAccountsHandler : IRequestHandler<AddDefaultAccountsCommand, AddDefaultAccountsResult>
    {
        private readonly ILanguageHelper _languageHelper;
        private readonly IMediator _mediator;

        public AddDefaultAccountsHandler(IMediator mediator, ILanguageHelper languageHelper)
            => (_mediator, _languageHelper) = (mediator, languageHelper);

        public async Task<AddDefaultAccountsResult> Handle(AddDefaultAccountsCommand request, CancellationToken cancellationToken)
        {
            if (_languageHelper.UseRussianLanguage(request.Locale))
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