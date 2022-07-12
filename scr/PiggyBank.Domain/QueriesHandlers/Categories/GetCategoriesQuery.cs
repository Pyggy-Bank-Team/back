using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Results.Models.Dto;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.QueriesHandlers.Categories
{
    public class GetCategoriesQuery : BaseQuery<CategoryDto[]>
    {
        private readonly Guid _userId;
        private readonly bool _all;

        public GetCategoriesQuery(PiggyContext context, Guid userId, bool all)
            : base(context)
            => (_userId, _all) = (userId, all);

        public override Task<CategoryDto[]> Invoke(CancellationToken token)
        {
            var query = _all
                ? GetRepository<Category>().Where(c => c.CreatedBy == _userId)
                : GetRepository<Category>().Where(c => c.CreatedBy == _userId && !c.IsDeleted);

            return query.Select(c => new CategoryDto
            {
                Id = c.Id,
                HexColor = c.HexColor,
                Title = c.Title,
                Type = c.Type,
                IsArchived = c.IsArchived,
                IsDeleted = c.IsDeleted,
                CreatedOn = c.CreatedOn,
                CreatedBy = c.CreatedBy
            }).ToArrayAsync(token);
        }
    }
}