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
    public class UpdatePartialBudgetOperationHandler : BaseHandler<UpdatePartialBidgetOperationCommand>
    {
        public UpdatePartialBudgetOperationHandler(PiggyContext context, UpdatePartialBidgetOperationCommand command) 
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<BudgetOperation>();
            var operation = await repository.FirstOrDefaultAsync(o => o.Id == Command.Id, token);

            if (operation == null)
            {
                //TODO: Add a log
                throw new ArgumentException($"Can't find operation with {Command.Id}");
            }

            //If the amount was changed then I'm undo the last change and 
            //Confirm the current amount
            if (Command.Amount.HasValue && operation.Amount != Command.Amount)
            {
                var account = await GetRepository<Account>().FirstOrDefaultAsync(a => !a.IsArchived && a.Id == Command.AccountId, token) ?? operation.Account;
                var category = await GetRepository<Category>().FirstOrDefaultAsync(c => !c.IsArchived && c.Id == Command.CategoryId, token) ?? operation.Category;

                var snapshot = JsonConvert.DeserializeObject<OperationSnapshot>(operation.Snapshot);
                account.ChangeBalance(snapshot.CategoryType == CategoryType.Income ? -operation.Amount : operation.Amount);
                account.ChangeBalance(category.Type == CategoryType.Income ? Command.Amount.Value : -Command.Amount.Value);
                GetRepository<Account>().Update(account);
            }

            operation.Amount = Command.Amount ?? operation.Amount;
            operation.Comment =  string.IsNullOrWhiteSpace(Command.Comment) ? operation.Comment : Command.Comment;
            operation.CategoryId = Command.CategoryId ?? operation.CategoryId;
            operation.AccountId = Command.AccountId ?? operation.AccountId;
            operation.ModifiedBy = Command.ModifiedBy;
            operation.ModifiedOn = Command.ModifiedOn;
            operation.OperationDate = Command.OperationDate ?? operation.OperationDate;

            repository.Update(operation);
        }
    }
}