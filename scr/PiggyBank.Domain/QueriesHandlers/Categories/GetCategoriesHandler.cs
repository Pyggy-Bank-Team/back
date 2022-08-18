using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Queries;
using Common.Results.Categories;
using Common.Results.Models.Dto;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.QueriesHandlers.Categories
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, GetCategoriesResult>
    {
        private readonly ICategoryRepository _repository;

        public GetCategoriesHandler(ICategoryRepository repository)
            => _repository = repository;

        public async Task<GetCategoriesResult> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _repository.GetAllAsync(request.UserId, cancellationToken);
            return new GetCategoriesResult
            {
                Data = categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Type = c.Type,
                    CreatedBy = c.CreatedBy,
                    CreatedOn = c.CreatedOn,
                    HexColor = c.HexColor,
                    IsArchived = c.IsArchived,
                    IsDeleted = c.IsDeleted
                }).ToArray()
            };
        }
    }
}