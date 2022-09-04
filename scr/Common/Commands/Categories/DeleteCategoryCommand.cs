using Common.Results.Categories;
using MediatR;

namespace Common.Commands.Categories
{
    public class DeleteCategoryCommand : BaseModifiedCommand, IRequest<DeleteCategoryResult>
    {
        public int Id { get; set; }
    }
}