using PiggyBank.Common.Commands.Categories;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto;
using PiggyBank.Domain.Handler.Categories;
using PiggyBank.Domain.Queries.Categories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PiggyBank.Domain.Services
{
    public partial class PiggyService : ICategoryService
    {
        public Task AddCategory(AddCategoryCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<AddCategoryHandler, AddCategoryCommand>(command, token);

        public Task ArchiveCategory(int id, CancellationToken token)
            => _handlerDispatcher.Invoke<ArchiveCategoryHandler, int>(id, token);

        public Task DeleteCategory(int id, CancellationToken token)
            => _handlerDispatcher.Invoke<DeleteCategoryHandler, int>(id, token);

        public Task<CategoryDto[]> GetCategories(Guid userId, bool all, CancellationToken token)
            => _queryDispatcher.Invoke<GetCategoriesQuery, CategoryDto[]>(userId, all);

        public Task<CategoryDto> GetCategory(int id, CancellationToken token)
            => _queryDispatcher.Invoke<GetCategoryByIdQuery, CategoryDto>(id);

        public Task PartialUpdateCategory(PartialUpdateCategoryCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<PartialUpdateCategoryHandler, PartialUpdateCategoryCommand>(command, token);

        public Task UpdateCategory(UpdateCategoryCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<UpdateCategoryHandler, UpdateCategoryCommand>(command, token);
    }
}
