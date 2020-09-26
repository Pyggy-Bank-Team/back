using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Operations;
using PiggyBank.Common.Commands.Operations.Budget;
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
                return;

            //TODO: checking of CategoryId and AccountId
            operation.Amount = Command.Amount ?? operation.Amount;
            operation.Comment =  string.IsNullOrWhiteSpace(Command.Comment) ? operation.Comment : Command.Comment;
            operation.CategoryId = Command.CategoryId ?? operation.CategoryId;
            operation.AccountId = Command.AccountId ?? operation.AccountId;
            operation.ModifiedBy = Command.ModifiedBy;
            operation.ModifiedOn = Command.ModifiedOn;

            repository.Update(operation);
        }
    }
}