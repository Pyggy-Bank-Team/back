using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Categories;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto;
using PiggyBank.Domain.Handlers.Categories;
using PiggyBank.Domain.Queries.Categories;
using PiggyBank.Model;
using Serilog;

namespace PiggyBank.Domain.Services
{
    public class CategoryService : PiggyServiceBase, ICategoryService
    {
        public CategoryService(PiggyContext context, ILogger logger) : base(context, logger)
        {
        }
        
        public Task<CategoryDto> AddCategory(AddCategoryCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<AddCategoryHandler, AddCategoryCommand, CategoryDto>(command, token);

        public Task ArchiveCategory(ArchiveCategoryCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<ArchiveCategoryHandler, ArchiveCategoryCommand>(command, token);

        public Task DeleteCategories(DeleteCategoriesCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<DeleteCategoriesHandler, DeleteCategoriesCommand>(command, token);

        public Task AddCategoryBatch(AddCategoryBatchCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<AddCategoryBatchHandler, AddCategoryBatchCommand>(command, token);

        public Task DeleteCategory(DeleteCategoryCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<DeleteCategoryHandler, DeleteCategoryCommand>(command, token);

        public Task<CategoryDto[]> GetCategories(Guid userId, bool all, CancellationToken token)
            => QueryDispatcher.Invoke<GetCategoriesQuery, CategoryDto[]>(token, userId, all);

        public Task<CategoryDto> GetCategory(int id, CancellationToken token)
            => QueryDispatcher.Invoke<GetCategoryByIdQuery, CategoryDto>(token, id);

        public Task PartialUpdateCategory(PartialUpdateCategoryCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<PartialUpdateCategoryHandler, PartialUpdateCategoryCommand>(command, token);

        public Task UpdateCategory(UpdateCategoryCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<UpdateCategoryHandler, UpdateCategoryCommand>(command, token);
    }
}