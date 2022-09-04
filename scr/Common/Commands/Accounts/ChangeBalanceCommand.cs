using System;
using Common.Results.Accounts;
using MediatR;

namespace Common.Commands.Accounts
{
    public class ChangeBalanceCommand : IRequest<ChangeBalanceResult>
    {
        public int AccountId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}