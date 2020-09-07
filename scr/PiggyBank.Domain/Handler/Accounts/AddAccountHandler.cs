using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Common.Models.Dto;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Accounts
{
    public class AddAccountHandler : BaseHandler<AddAccountCommand>
    {
        public AddAccountHandler(PiggyContext context, AddAccountCommand command)
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var result = await GetRepository<Account>().AddAsync(new Account
            {
                Balance = Command.Balance,
                Currency = Command.Currency,
                IsArchived = Command.IsArchived,
                IsDeleted = Command.IsDeleted,
                Title = Command.Title,
                Type = Command.Type,
                CreatedBy = Command.CreatedBy,
                CreatedOn = Command.CreatedOn
            }, token);

            await SaveAsync();
            var account = result.Entity;
            Result = new AccountDto
            {
                Id = account.Id,
                Balance = account.Balance,
                Currency = account.Currency,
                Title = account.Title,
                Type = account.Type,
                IsArchived = account.IsArchived,
                IsDeleted = account.IsDeleted
            };
        }
    }
}
