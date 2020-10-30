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

        public override Task Invoke(CancellationToken token)
            => Task.Run(() =>
            {
                var repository = GetRepository<Account>();

                var ids = Command.Ids;
                foreach (var account in repository.Where(a => ids.Contains(a.Id)))
                {
                    account.IsDeleted = true;
                    account.ModifiedBy = Command.ModifiedBy;
                    account.ModifiedOn = Command.ModifiedOn;
                    repository.Update(account);
                    
                    //Delete all related operations

                    foreach (var budgetOperation in GetRepository<BudgetOperation>().Where(b => !b.IsDeleted && b.AccountId == account.Id))
                    {
                        budgetOperation.IsDeleted = true;
                        GetRepository<BudgetOperation>().Update(budgetOperation);
                    }
            
                    foreach (var transferOperation in GetRepository<TransferOperation>().Where(b => !b.IsDeleted && (b.From == account.Id || b.To == account.Id)))
                    {
                        transferOperation.IsDeleted = true;
                        GetRepository<TransferOperation>().Update(transferOperation);
                    }
            
                    foreach (var planOperation in GetRepository<PlanOperation>().Where(b => !b.IsDeleted && b.AccountId == account.Id))
                    {
                        planOperation.IsDeleted = true;
                        GetRepository<PlanOperation>().Update(planOperation);
                    }
                }
            }, token);
    }
}