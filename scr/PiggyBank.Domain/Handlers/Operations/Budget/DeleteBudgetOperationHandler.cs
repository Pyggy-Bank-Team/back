using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Operations.Budget;
using PiggyBank.Domain.InternalServices;
using PiggyBank.Model;

namespace PiggyBank.Domain.Handlers.Operations.Budget
{
    public class DeleteBudgetOperationHandler : BaseHandler<DeleteBudgetOperationCommand>
    {
        public DeleteBudgetOperationHandler(PiggyContext context, DeleteBudgetOperationCommand command)
            : base(context, command)
        {
        }

        public override Task Invoke(CancellationToken token)
        {
            var deletionService = new DeleteOperationService((PiggyContext)Context);
            return deletionService.DeleteBudgetOperation(Command.Id, Command, token);
        }
    }
}