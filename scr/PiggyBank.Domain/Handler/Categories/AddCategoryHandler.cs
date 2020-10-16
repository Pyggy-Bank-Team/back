using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Categories;
using PiggyBank.Common.Models.Dto;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Categories
{
    public class AddCategoryHandler : BaseHandler<AddCategoryCommand>
    {
        public AddCategoryHandler(PiggyContext context, AddCategoryCommand command)
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var result = await GetRepository<Category>().AddAsync(new Category
            {
                Title = Command.Title,
                HexColor = Command.HexColor,
                Type = Command.Type,
                CreatedBy = Command.CreatedBy,
                CreatedOn = Command.CreatedOn,
                IsArchived = Command.IsArchived
            }, token);

            await SaveAsync();
            var entity = result.Entity;
            Result = new CategoryDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Type = entity.Type,
                HexColor = entity.HexColor,
                IsArchived = entity.IsArchived,
                IsDeleted = entity.IsDeleted,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };
        }
    }
}
