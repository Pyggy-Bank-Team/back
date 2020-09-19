using Microsoft.EntityFrameworkCore;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Accounts;

namespace PiggyBank.Domain.Handler.Accounts
{
    public class ArchiveAccountHandler : BaseHandler<ArchiveAccountCommand>
    {
        public ArchiveAccountHandler(PiggyContext context, ArchiveAccountCommand command)
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<Account>();
            var account = await repository.FirstOrDefaultAsync(a => a.Id == Command.Id && !a.IsDeleted, cancellationToken: token);

            if (account == null || account.IsArchived)
                return;

            account.IsArchived = true;
            account.ModifiedBy = Command.ModifiedBy;
            account.ModifiedOn = Command.ModifiedOn;
            repository.Update(account);
        }
    }
}
