using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Categories;
using Common.Enums;
using Common.Results.Categories;
using MediatR;
using PiggyBank.Domain.Helpers;

namespace PiggyBank.Domain.Handlers.Categories
{
    public class AddDefaultCategoriesHandler : IRequestHandler<AddDefaultCategoriesCommand, AddDefaultCategoriesResult>
    {
        private readonly IMediator _mediator;
        private readonly ILanguageHelper _languageHelper;

        public AddDefaultCategoriesHandler(IMediator mediator, ILanguageHelper languageHelper)
            => (_mediator, _languageHelper) = (mediator, languageHelper);

        public async Task<AddDefaultCategoriesResult> Handle(AddDefaultCategoriesCommand request, CancellationToken cancellationToken)
        {
            if (_languageHelper.UseRussianLanguage(request.Locale))
            {
                foreach (var command in GenerateCategoryCommandsWithRussiansTitle())
                {
                    _ = await _mediator.Send(command, cancellationToken);
                }
            }
            else
            {
                foreach (var command in GenerateCategoryCommandsWithEnglishTitle())
                {
                    _ = await _mediator.Send(command, cancellationToken);
                }
            }

            return new AddDefaultCategoriesResult();
        }

        private IEnumerable<AddCategoryCommand> GenerateCategoryCommandsWithRussiansTitle()
        {
            yield return new AddCategoryCommand
            {
                Title = "Доход",
                Type = CategoryType.Income,
                HexColor = "#bf0077",
                IsArchived = false
            };
                
            yield return new AddCategoryCommand
            {
                Title = "Расход",
                Type = CategoryType.Expense,
                HexColor = "#0078d7",
                IsArchived = false
            };
        }
        
        private IEnumerable<AddCategoryCommand> GenerateCategoryCommandsWithEnglishTitle()
        {
            yield return new AddCategoryCommand
            {
                Title = "Income",
                Type = CategoryType.Income,
                HexColor = "#bf0077",
                IsArchived = false
            };
                
            yield return new AddCategoryCommand
            {
                Title = "Expense",
                Type = CategoryType.Expense,
                HexColor = "#0078d7",
                IsArchived = false
            };
        }
    }
}