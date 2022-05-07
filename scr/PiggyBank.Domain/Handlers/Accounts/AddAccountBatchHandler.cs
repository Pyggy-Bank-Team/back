using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handlers.Accounts
{
    public class AddAccountBatchHandler : BaseHandler<AddAccountBatchCommand>
    {
        public AddAccountBatchHandler(PiggyContext context, AddAccountBatchCommand command) : base(context, command)
        {
        }

        public override Task Invoke(CancellationToken token)
        {
            var accounts = Command.Accounts.Select(i => new Account
            {
                Balance = i.Balance,
                Currency = i.Currency,
                IsArchived = i.IsArchived,
                Title = i.Title,
                Type = i.Type,
                CreatedBy = Command.CreatedBy,
                CreatedOn = Command.CreatedOn
            }).ToArray();
            
            return GetRepository<Account>().AddRangeAsync(accounts, token);
        }
    }
}