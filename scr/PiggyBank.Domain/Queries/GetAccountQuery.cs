using System;
using MediatR;
using PiggyBank.Domain.Results.Accounts;

namespace PiggyBank.Domain.Queries
{
    public class GetAccountQuery : IRequest<GetAccountResult>
    {
        public int AccountId { get; set; }
        public Guid UserId { get; set; }
    }
}