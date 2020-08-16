using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Operations.Plan;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Operations.Plan
{
    public class UpdatePartialPlanOperationHandler : BaseHandler<UpdatePartialPlanOperationCommand>
    {
        public UpdatePartialPlanOperationHandler(PiggyContext context, UpdatePartialPlanOperationCommand command)
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<PlanOperation>();
            var operation = await repository.FirstOrDefaultAsync(o => o.Id == Command.Id, token);

            if (operation == null)
                return;
            
            operation.Amount = Command.Amount ?? operation.Amount;
            operation.Comment = string.IsNullOrWhiteSpace(Command.Comment) ? operation.Comment : Command.Comment;
            operation.CategoryId = Command.CategoryId ?? operation.CategoryId;
            operation.AccountId = Command.AccountId ?? operation.AccountId;
            operation.PlanDate = Command.PlanDate ?? operation.PlanDate;

            repository.Update(operation);
        }
    }
}