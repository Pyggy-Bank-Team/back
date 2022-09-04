using System;
using MediatR;
using Common.Results.Accounts;

namespace Common.Queries
{
    public class GetAccountQuery : IRequest<GetAccountResult>
    {
        public int AccountId { get; set; }
        public Guid UserId { get; set; }
    }
}