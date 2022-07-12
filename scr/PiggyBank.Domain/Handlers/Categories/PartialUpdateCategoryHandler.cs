using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Commands.Categories;
using Common.Results.Categories;
using Common.Results.Models.Dto;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.Handlers.Categories
{
    public class PartialUpdateCategoryHandler : IRequestHandler<PartialUpdateCategoryCommand, PartialUpdateCategoryResult>
    {
        private readonly ICategoryRepository _repository;

        public PartialUpdateCategoryHandler(ICategoryRepository repository)
            => _repository = repository;

        public async Task<PartialUpdateCategoryResult> Handle(PartialUpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetAsync(request.ModifiedBy, request.Id, cancellationToken);

            if (category == null || category.IsDeleted)
                return new PartialUpdateCategoryResult{ErrorCode = ErrorCodes.InvalidRequest, Messages = new []{"Category not found or deleted"}};

            category.Title = string.IsNullOrWhiteSpace(request.Title) ? category.Title : request.Title;
            category.HexColor = string.IsNullOrWhiteSpace(request.HexColor) ? category.HexColor : request.HexColor;
            category.IsArchived = request.IsArchived ?? category.IsArchived;
            category.ModifiedBy = request.ModifiedBy;
            category.ModifiedOn = request.ModifiedOn;

            var updatedCategory = await _repository.UpdateAsync(category, cancellationToken);
            return new PartialUpdateCategoryResult
            {
                Data = new CategoryDto
                {
                    Id = updatedCategory.Id,
                    Title = updatedCategory.Title,
                    Type = updatedCategory.Type,
                    CreatedBy = updatedCategory.CreatedBy,
                    CreatedOn = updatedCategory.CreatedOn,
                    HexColor = updatedCategory.HexColor,
                    IsArchived = updatedCategory.IsArchived,
                    IsDeleted = updatedCategory.IsDeleted
                }
            };
        }
    }
}
