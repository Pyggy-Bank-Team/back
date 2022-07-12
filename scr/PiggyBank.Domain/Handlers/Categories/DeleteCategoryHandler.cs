using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Commands.Categories;
using Common.Queries;
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
        private readonly IMediator _mediator;

        public DeleteCategoryHandler(ICategoryRepository repository, ILanguageHelper languageHelper, IMediator mediator)
            => (_repository, _languageHelper, _mediator) = (repository, languageHelper, mediator);

        public async Task<DeleteCategoryResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetAsync(request.ModifiedBy, request.Id, cancellationToken);

            if (category == null)
                return new DeleteCategoryResult{ErrorCode = ErrorCodes.NotFound};

            if (category.IsDeleted)
                return new DeleteCategoryResult();

            var appSettings = await _mediator.Send(new GetAppSettingsQuery { UserId = request.ModifiedBy }, cancellationToken);

            category.IsDeleted = true;
            category.Title = _languageHelper.UseRussianLanguage(appSettings.Data?.Locale) ? "Без категории" : "Deleted";
            category.HexColor = CategoryColors.White;
            category.ModifiedBy = request.ModifiedBy;
            category.ModifiedOn = request.ModifiedOn;

            _ = await _repository.UpdateAsync(category, cancellationToken);
            return new DeleteCategoryResult();
        }
    }
}