using Common.Results.Categories;
using MediatR;

namespace Common.Commands.Categories
{
    public class AddDefaultCategoriesCommand : BaseCreateCommand, IRequest<AddDefaultCategoriesResult>
    {
        public string Locale { get; set; }
    }
}