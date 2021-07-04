using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Categories;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PiggyBank.Domain.Handler.Categories
{
    public class UpdateCategoryHandler : BaseHandler<UpdateCategoryCommand>
    {
        public UpdateCategoryHandler(PiggyContext context, UpdateCategoryCommand command)
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<Category>();
            var category = await repository.FirstOrDefaultAsync(a => a.Id == Command.Id, token);

            if (category == null)
                return;

            category.Title = Command.Title;
            category.HexColor = Command.HexColor;
            category.IsArchived = Command.IsArchived;
            category.ModifiedBy = Command.ModifiedBy;
            category.ModifiedOn = Command.ModifiedOn;

            repository.Update(category);
        }
    }
}
