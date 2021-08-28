using System;
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
    public class UpdateBudgetOperationHandler : BaseHandler<UpdateBidgetOperationCommand>
    {
        public UpdateBudgetOperationHandler(PiggyContext context, UpdateBidgetOperationCommand command)
            : base(context, command)
        {
        }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<BudgetOperation>();
            var operation = await repository.FirstOrDefaultAsync(o => o.Id == Command.Id, token);

            if (operation == null)
            {
                //TODO: Add a log
                throw new ArgumentException($"Can't find operation with {Command.Id}");
            }

            var account = await GetRepository<Account>().FirstOrDefaultAsync(a => !a.IsArchived && a.Id == Command.AccountId, token);
            if (account == null)
            {
                //TODO: Add a log
                throw new ArgumentException($"Can't find account with {Command.AccountId}");
            }

            var category = await GetRepository<Category>().FirstOrDefaultAsync(c => !c.IsArchived && c.Id == Command.CategoryId, token);
            if (category == null)
            {
                //TODO: Add a log
                throw new ArgumentException($"Can't find category with {Command.AccountId}");
            }

            //If the amount was changed then I undo the last change and 
            //Confirm the current amount
            if (operation.Amount != Command.Amount)
            {
                var snapshot = JsonConvert.DeserializeObject<OperationSnapshot>(operation.Snapshot);

                account.ChangeBalance(snapshot.CategoryType == CategoryType.Income ? -operation.Amount : operation.Amount);
                account.ChangeBalance(category.Type == CategoryType.Income ? Command.Amount : -Command.Amount);
                GetRepository<Account>().Update(account);
            }

            operation.Amount = Command.Amount;
            operation.Comment = Command.Comment;
            operation.CategoryId = Command.CategoryId;
            operation.AccountId = Command.AccountId;
            operation.ModifiedBy = Command.ModifiedBy;
            operation.ModifiedOn = Command.ModifiedOn;
            operation.OperationDate = Command.OperationDate;

            repository.Update(operation);
        }
    }
}