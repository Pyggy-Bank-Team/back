using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Operations.Plan;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Operations.Plan
{
    public class DeletePlanOperationHandler : BaseHandler<DeletePlanOperationCommand>
    {
        public DeletePlanOperationHandler(PiggyContext context, DeletePlanOperationCommand command)
            : base(context, command)
        {
        }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<PlanOperation>();
            var operation = await repository.FirstOrDefaultAsync(p => p.Id == Command.Id, token);

            if (operation != null)
            {
                operation.IsDeleted = true;
                operation.ModifiedBy = Command.ModifiedBy;
                operation.ModifiedOn = Command.ModifiedOn;
                repository.Update(operation);
            }
        }
    }
}