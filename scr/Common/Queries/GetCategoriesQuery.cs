using System;
using Common.Results.Categories;
using MediatR;

namespace Common.Queries
{
    public class GetCategoriesQuery : IRequest<GetCategoriesResult>
    {
        public Guid UserId { get; set; }
    }
}