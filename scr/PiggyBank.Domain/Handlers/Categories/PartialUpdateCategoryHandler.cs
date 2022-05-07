using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Categories;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handlers.Categories
{
    public class PartialUpdateCategoryHandler : BaseHandler<PartialUpdateCategoryCommand>
    {
        public PartialUpdateCategoryHandler(PiggyContext context, PartialUpdateCategoryCommand command)
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<Category>();
            var category = await repository.FirstOrDefaultAsync(a => a.Id == Command.Id, token);

            if (category == null)
                return;

            category.Title = GetOldValueOrNewValue(category.Title, Command.Title);
            category.HexColor = GetOldValueOrNewValue(category.HexColor, Command.HexColor);
            category.IsArchived = Command.IsArchived ?? category.IsArchived;
            category.ModifiedBy = Command.ModifiedBy;
            category.ModifiedOn = Command.ModifiedOn;

            repository.Update(category);
        }

        private static string GetOldValueOrNewValue(string oldValue, string newValue)
            => string.IsNullOrWhiteSpace(newValue) ? oldValue : newValue;
    }
}
