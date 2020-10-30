using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Accounts
{
    public class DeleteAccountsHandler : BaseHandler<DeleteAccountsCommand>
    {
        public DeleteAccountsHandler(PiggyContext context, DeleteAccountsCommand command)
            : base(context, command)
        {
        }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<Account>();

            var ids = Command.Ids;
            foreach (var account in repository.Where(a => ids.Contains(a.Id)))
            {
                account.IsDeleted = true;
                account.ModifiedBy = Command.ModifiedBy;
                account.ModifiedOn = Command.ModifiedOn;
                repository.Update(account);
            }
        }
    }
}