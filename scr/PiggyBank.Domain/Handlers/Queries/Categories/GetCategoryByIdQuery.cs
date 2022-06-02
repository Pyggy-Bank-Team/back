using Microsoft.EntityFrameworkCore;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Results.Models.Dto;

namespace PiggyBank.Domain.Queries.Categories
{
    public class GetCategoryByIdQuery : BaseQuery<CategoryDto>
    {
        private readonly int _categoryId;
        public GetCategoryByIdQuery(PiggyContext context, int categoryId)
            : base(context)
            => _categoryId = categoryId;

        public override Task<CategoryDto> Invoke(CancellationToken token)
            => GetRepository<Category>().Where(c => c.Id == _categoryId && !c.IsDeleted)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                HexColor = c.HexColor,
                Title = c.Title,
                Type = c.Type
            }).FirstOrDefaultAsync(token);
    }
}
