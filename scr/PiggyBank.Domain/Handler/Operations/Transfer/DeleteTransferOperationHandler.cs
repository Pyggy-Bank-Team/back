using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Operations.Transfer;
using PiggyBank.Domain.InternalServices;
using PiggyBank.Model;

namespace PiggyBank.Domain.Handler.Operations.Transfer
{
    public class DeleteTransferOperationHandler : BaseHandler<DeleteTransferOperationCommand>
    {
        public DeleteTransferOperationHandler(PiggyContext context, DeleteTransferOperationCommand command)
            : base(context, command) { }

        public override Task Invoke(CancellationToken token)
        {
            var deletionService = new DeleteOperationService(Context);
            return deletionService.DeleteTransferOperation(Command.Id, Command, token);
        }
    }
}
