using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Categories;
using Common.Results.Categories;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.Handlers.Categories
{
    public class DeleteCategoriesHandler : IRequestHandler<DeleteCategoriesCommand, DeleteCategoryResult>
    {
        private readonly ICategoryRepository _repository;
        private readonly IMediator _mediator;

        public DeleteCategoriesHandler(ICategoryRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<DeleteCategoryResult> Handle(DeleteCategoriesCommand request, CancellationToken cancellationToken)
        {
            foreach (var categoryId in request.Ids)
            {
                var deleteCategoryCommand = new DeleteCategoryCommand { Id = categoryId, ModifiedBy = request.ModifiedBy, ModifiedOn = request.ModifiedOn };
                var deleteCategoryResult = await _mediator.Send(deleteCategoryCommand, cancellationToken);

                if (!deleteCategoryResult.IsSuccess)
                    return new DeleteCategoryResult { ErrorCode = deleteCategoryResult.ErrorCode, Messages = deleteCategoryResult.Messages};
            }

            return new DeleteCategoryResult();
        }
    }
}