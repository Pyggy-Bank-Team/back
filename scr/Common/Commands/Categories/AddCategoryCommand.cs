using Common.Enums;
using Common.Results.Categories;
using MediatR;

namespace Common.Commands.Categories
{
    public class AddCategoryCommand : BaseCreateCommand, IRequest<AddCategoryResult>
    {
        public string Title { get; set; }

        public string HexColor { get; set; }

        public CategoryType Type { get; set; }

        public bool IsArchived { get; set; }
    }
}
