using System;
using MediatR;
using PiggyBank.Domain.Results.Accounts;

namespace PiggyBank.Domain.Queries
{
    public class GetAccountsQuery : IRequest<GetAccountsResult>
    {
        public bool All { get; set; }
        public Guid UserId { get; set; }
    }
}