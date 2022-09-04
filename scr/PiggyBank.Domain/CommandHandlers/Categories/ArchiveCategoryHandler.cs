using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Commands.Categories;
using Common.Results.Categories;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.CommandHandlers.Categories
{
    public class ArchiveCategoryHandler : IRequestHandler<ArchiveCategoryCommand, ArchiveCategoryResult>
    {
        private readonly ICategoryRepository _repository;

        public ArchiveCategoryHandler(ICategoryRepository repository)
            => _repository = repository;

        public async Task<ArchiveCategoryResult> Handle(ArchiveCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetAsync(request.ModifiedBy, request.Id, cancellationToken);

            if (category == null)
                return new ArchiveCategoryResult { ErrorCode = ErrorCodes.NotFound };

            if (category.IsArchived || category.IsDeleted)
                return new ArchiveCategoryResult { ErrorCode = ErrorCodes.InvalidRequest, Messages = new[] { "Category is already archived or deleted" } };

            category.IsArchived = true;
            category.ModifiedBy = request.ModifiedBy;
            category.ModifiedOn = request.ModifiedOn;

            _ = await _repository.UpdateAsync(category, cancellationToken);
            return new ArchiveCategoryResult();
        }
    }
}