using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Operations;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Operations.Budget
{
    public class UpdateBudgetOperationHandler : BaseHandler<UpdateBidgetOperationCommand>
    {
        public UpdateBudgetOperationHandler(PiggyContext context, UpdateBidgetOperationCommand command) 
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<BudgetOperation>();
            var operation = await repository.FirstOrDefaultAsync(o => o.Id == Command.Id, token);

            if (operation == null)
                return;

            //TODO: checking of CategoryId and AccountId
            operation.Amount = Command.Amount;
            operation.Comment = Command.Comment;
            operation.CategoryId = Command.CategoryId;
            operation.AccountId = Command.AccountId;

            repository.Update(operation);
        }
    }
}
