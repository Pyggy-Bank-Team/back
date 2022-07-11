using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Commands.Categories;
using Common.Results.Categories;
using MediatR;
using PiggyBank.Domain.Helpers;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.Handlers.Categories
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, DeleteCategoryResult>
    {
        private readonly ICategoryRepository _repository;
        private readonly ILanguageHelper _languageHelper;

        public DeleteCategoryHandler(ICategoryRepository repository, ILanguageHelper languageHelper)
            => (_repository, _languageHelper) = (repository, languageHelper);

        public async Task<DeleteCategoryResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetAsync(request.ModifiedBy, request.Id, cancellationToken);

            if (category == null)
                return new DeleteCategoryResult{ErrorCode = ErrorCodes.NotFound};

            if (category.IsDeleted)
                return new DeleteCategoryResult();

            category.IsDeleted = true;
            category.Title = _languageHelper.UseRussianLanguage(request.Locale) ? "Без категории" : "Deleted";
            category.HexColor = CategoryColors.White;
            category.ModifiedBy = request.ModifiedBy;
            category.ModifiedOn = request.ModifiedOn;

            _ = await _repository.UpdateAsync(category, cancellationToken);
            return new DeleteCategoryResult();
        }
    }
}