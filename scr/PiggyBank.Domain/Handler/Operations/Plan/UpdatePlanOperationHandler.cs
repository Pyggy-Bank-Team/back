using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Operations.Plan;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Operations.Plan
{
    public class UpdatePlanOperationHandler : BaseHandler<UpdatePlanOperationCommand>
    {
        public UpdatePlanOperationHandler(PiggyContext context, UpdatePlanOperationCommand command)
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<PlanOperation>();
            var operation = await repository.FirstOrDefaultAsync(o => o.Id == Command.Id, token);

            if (operation == null)
                return;
            
            operation.Amount = Command.Amount;
            operation.Comment = Command.Comment;
            operation.CategoryId = Command.CategoryId;
            operation.AccountId = Command.AccountId;
            operation.PlanDate = Command.PlanDate;

            repository.Update(operation);
        }
    }
}