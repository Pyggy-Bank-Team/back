using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PiggyBank.Common.Commands.Operations.Budget;
using PiggyBank.Common.Enums;
using PiggyBank.Domain.Models.Operations;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Operations.Budget
{
    public class DeleteBudgetOperationHandler : BaseHandler<DeleteBudgetOperationCommand>
    {
        public DeleteBudgetOperationHandler(PiggyContext context, DeleteBudgetOperationCommand command)
            : base(context, command)
        {
        }

        public override async Task Invoke(CancellationToken token)
        {
            var operationRepository = GetRepository<BudgetOperation>();
            var accountRepository = GetRepository<Account>();

            var operation = await operationRepository.FirstOrDefaultAsync(o => o.Id == Command.Id && !o.IsDeleted, token);

            if (operation == null)
                return;

            operation.IsDeleted = true;
            operation.ModifiedBy = Command.ModifiedBy;
            operation.ModifiedOn = Command.ModifiedOn;
            operationRepository.Update(operation);

            var account = accountRepository.FirstOrDefault(a => a.Id == operation.AccountId
                                                                && !a.IsDeleted && !a.IsArchived);

            if (account != null)
            {
                var snapshot = JsonConvert.DeserializeObject<OperationSnapshot>(operation.Snapshot);

                account.ChangeBalance(snapshot.CategoryType == CategoryType.Income ? -operation.Amount : operation.Amount);
                accountRepository.Update(account);
            }
        }
    }
}