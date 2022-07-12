using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Operations.Transfer;
using PiggyBank.Domain.InternalServices;
using PiggyBank.Model;

namespace PiggyBank.Domain.CommandHandlers.Operations.Transfer
{
    public class DeleteTransferOperationHandler : BaseHandler<DeleteTransferOperationCommand>
    {
        public DeleteTransferOperationHandler(PiggyContext context, DeleteTransferOperationCommand command)
            : base(context, command) { }

        public override Task Invoke(CancellationToken token)
        {
            var deletionService = new DeleteOperationService((PiggyContext)Context);
            return deletionService.DeleteTransferOperation(Command.Id, Command, token);
        }
    }
}
