using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Categories;
using Common.Results.Categories;
using Common.Results.Models.Dto;
using MediatR;
using PiggyBank.Model.Interfaces;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.CommandHandlers.Categories
{
    public class AddCategoryHandler : IRequestHandler<AddCategoryCommand, AddCategoryResult>
    {
        private readonly ICategoryRepository _repository;

        public AddCategoryHandler(ICategoryRepository repository)
            => _repository = repository;

        public async Task<AddCategoryResult> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var newCategory = new Category
            {
                Title = request.Title,
                Type = request.Type,
                CreatedBy = request.CreatedBy,
                CreatedOn = request.CreatedOn,
                HexColor = request.HexColor,
                IsArchived = request.IsArchived
            };

            var createdCategory = await _repository.AddAsync(newCategory, cancellationToken);

            return new AddCategoryResult
            {
                Data = new CategoryDto
                {
                    Id = createdCategory.Id,
                    Title = createdCategory.Title,
                    Type = createdCategory.Type,
                    CreatedBy = createdCategory.CreatedBy,
                    CreatedOn = createdCategory.CreatedOn,
                    HexColor = createdCategory.HexColor,
                    IsArchived = createdCategory.IsArchived,
                    IsDeleted = createdCategory.IsDeleted
                }
            };
        }
    }
}