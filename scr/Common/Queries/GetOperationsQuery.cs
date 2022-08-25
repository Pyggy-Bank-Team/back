using System;
using Common.Results;
using Common.Results.Models.Dto.Operations;
using MediatR;

namespace Common.Queries
{
    public class GetOperationsQuery : IRequest<PageResult<OperationDto>>
    {
        public Guid UserId { get; set; }
        public int Page { get; set; }
    }
}