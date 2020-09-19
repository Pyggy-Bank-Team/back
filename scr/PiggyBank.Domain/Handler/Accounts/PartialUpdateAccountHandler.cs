using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PiggyBank.Domain.Handler.Accounts
{
    public class PartialUpdateAccountHandler : BaseHandler<PartialUpdateAccountCommand>
    {
        public PartialUpdateAccountHandler(PiggyContext context, PartialUpdateAccountCommand command)
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var account = await GetRepository<Account>().FirstOrDefaultAsync(a => a.Id == Command.Id, token);

            if (account == null)
                return;

            account.Title = string.IsNullOrWhiteSpace(Command.Title) ? account.Title : Command.Title;
            account.Type = Command.Type ?? account.Type;
            account.Balance = Command.Balance ?? account.Balance;
            //account.Currency = string.IsNullOrWhiteSpace(Command.Currency) ? account.Currency : Command.Currency;
            account.IsArchived = Command.IsArchive ?? account.IsArchived;
            account.ModifiedBy = Command.ModifiedBy;
            account.ModifiedOn = Command.ModifiedOn;

            GetRepository<Account>().Update(account);
        }
    }
}