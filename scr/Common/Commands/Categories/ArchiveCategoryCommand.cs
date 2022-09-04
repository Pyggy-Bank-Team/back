using Common.Results.Categories;
using MediatR;

namespace Common.Commands.Categories
{
    public class ArchiveCategoryCommand : BaseModifiedCommand, IRequest<ArchiveCategoryResult>
    {
        public int Id { get; set; }
    }
}