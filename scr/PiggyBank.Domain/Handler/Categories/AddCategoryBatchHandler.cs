using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Categories;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Categories
{
    public class AddCategoryBatchHandler : BaseHandler<AddCategoryBatchCommand>
    {
        public AddCategoryBatchHandler(PiggyContext context, AddCategoryBatchCommand command) : base(context, command)
        {
        }

        public override Task Invoke(CancellationToken token)
        {
            var categories = Command.Categories.Select(i => new Category
            {
                Title = i.Title,
                HexColor = i.HexColor,
                Type = i.Type,
                IsArchived = i.IsArchived,
                CreatedBy = Command.CreatedBy,
                CreatedOn = Command.CreatedOn
            }).ToArray();
            return GetRepository<Category>().AddRangeAsync(categories);
        }
    }
}