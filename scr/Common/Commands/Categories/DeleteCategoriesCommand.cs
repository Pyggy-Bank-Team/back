using Common.Results.Categories;
using MediatR;

namespace Common.Commands.Categories
{
    public class DeleteCategoriesCommand : BaseModifiedCommand, IRequest<DeleteCategoryResult>
    {
        public int[] Ids { get; set; }
    }
}