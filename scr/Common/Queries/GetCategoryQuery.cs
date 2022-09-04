using System;
using Common.Results.Categories;
using MediatR;

namespace Common.Queries
{
    public class GetCategoryQuery : IRequest<GetCategoryResult>
    {
        public int CategoryId { get; set; }
        public Guid UserId { get; set; }
    }
}