using System.Linq;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Categories;

namespace PiggyBank.Domain.Handler.Categories
{
    public class DeleteCategoriesHandler : BaseHandler<DeleteCategoriesCommand>
    {
        public DeleteCategoriesHandler(PiggyContext context, DeleteCategoriesCommand command)
            : base(context, command)
        {
        }

        public override Task Invoke(CancellationToken token)
            => Task.Run(() =>
            {
                var repository = GetRepository<Category>();

                var ids = Command.Ids;
                foreach (var category in repository.Where(c => !c.IsDeleted && ids.Contains(c.Id)))
                {
                    category.IsDeleted = true;
                    category.ModifiedBy = Command.ModifiedBy;
                    category.ModifiedOn = Command.ModifiedOn;
                    GetRepository<Category>().Update(category);
                }
            }, token);
    }
}