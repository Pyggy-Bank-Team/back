using Microsoft.EntityFrameworkCore;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Categories;

namespace PiggyBank.Domain.Handler.Categories
{
    public class DeleteCategoryHandler : BaseHandler<DeleteCategoryCommand>
    {
        public DeleteCategoryHandler(PiggyContext context, DeleteCategoryCommand command)
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<Category>();
            var category = await repository.FirstOrDefaultAsync(c => c.Id == Command.Id && !c.IsDeleted, token);

            if (category == null)
                return;

            category.IsDeleted = true;
            category.Title = "Deleted";
            category.HexColor = "#FFFFFF";
            category.ModifiedBy = Command.ModifiedBy;
            category.ModifiedOn = Command.ModifiedOn;
            repository.Update(category);
        }
    }
}
