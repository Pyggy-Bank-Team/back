using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Models.Dto;
using PiggyBank.Domain.Queries;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handlers.Accounts
{
    public class GetAccountByIdQuery : BaseQuery<AccountDto>
    {
        private readonly int _accountId;
        public GetAccountByIdQuery(PiggyContext context, int accountId) : base(context)
            => _accountId = accountId;

        public override Task<AccountDto> Invoke(CancellationToken token)
            => GetRepository<Account>().Where(a => a.Id == _accountId && !a.IsDeleted)
            .Select(a => new AccountDto
            {
                Id = a.Id,
                Balance = a.Balance,
                Currency = a.Currency,
                IsArchived = a.IsArchived,
                IsDeleted = a.IsDeleted,
                Title = a.Title,
                Type = a.Type,
                CreatedOn = a.CreatedOn,
                CreatedBy = a.CreatedBy
            }).FirstOrDefaultAsync(token);
    }
}
