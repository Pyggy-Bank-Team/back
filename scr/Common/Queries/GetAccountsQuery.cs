using System;
using MediatR;
using Common.Results.Accounts;

namespace Common.Queries
{
    public class GetAccountsQuery : IRequest<GetAccountsResult>
    {
        public bool All { get; set; }
        public Guid UserId { get; set; }
    }
}