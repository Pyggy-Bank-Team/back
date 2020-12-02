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
        public Task<CategoryDto> AddCategory(AddCategoryCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<AddCategoryHandler, AddCategoryCommand, CategoryDto>(command, token);

        public Task ArchiveCategory(ArchiveCategoryCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<ArchiveCategoryHandler, ArchiveCategoryCommand>(command, token);

        public Task DeleteCategories(DeleteCategoriesCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<DeleteCategoriesHandler, DeleteCategoriesCommand>(command, token);

        public Task AddCategoryBatch(AddCategoryBatchCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<AddCategoryBatchHandler, AddCategoryBatchCommand>(command, token);

        public Task DeleteCategory(DeleteCategoryCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<DeleteCategoryHandler, DeleteCategoryCommand>(command, token);

        public Task<CategoryDto[]> GetCategories(Guid userId, bool all, CancellationToken token)
            => _queryDispatcher.Invoke<GetCategoriesQuery, CategoryDto[]>(token, userId, all);

        public Task<CategoryDto> GetCategory(int id, CancellationToken token)
            => _queryDispatcher.Invoke<GetCategoryByIdQuery, CategoryDto>(token, id);

        public Task PartialUpdateCategory(PartialUpdateCategoryCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<PartialUpdateCategoryHandler, PartialUpdateCategoryCommand>(command, token);

        public Task UpdateCategory(UpdateCategoryCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<UpdateCategoryHandler, UpdateCategoryCommand>(command, token);
    }
}
