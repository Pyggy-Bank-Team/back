using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Queries;
using Common.Results.Categories;
using Common.Results.Models.Dto;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.QueriesHandlers.Categories
{
    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, GetCategoryResult>
    {
        private readonly ICategoryRepository _repository;
        public GetCategoryHandler(ICategoryRepository repository)
            => _repository = repository;

        public async Task<GetCategoryResult> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetAsync(request.UserId, request.CategoryId, cancellationToken);

            if (category == null)
                return new GetCategoryResult { ErrorCode = ErrorCodes.NotFound };

            return new GetCategoryResult
            {
                Data = new CategoryDto
                {
                    Id = category.Id,
                    Title = category.Title,
                    Type = category.Type,
                    CreatedBy = category.CreatedBy,
                    CreatedOn = category.CreatedOn,
                    HexColor = category.HexColor,
                    IsArchived = category.IsArchived,
                    IsDeleted = category.IsDeleted
                }
            };
        }
    }
}