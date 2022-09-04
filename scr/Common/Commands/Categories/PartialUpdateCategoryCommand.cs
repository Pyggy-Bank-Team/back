using Common.Results.Categories;
using MediatR;

namespace Common.Commands.Categories
{
    public class PartialUpdateCategoryCommand : BaseModifiedCommand, IRequest<PartialUpdateCategoryResult>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string HexColor { get; set; }
        public bool? IsArchived { get; set; }
    }
}
