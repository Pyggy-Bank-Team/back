using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Operations;
using Common.Enums;
using PiggyBank.Domain.InternalServices;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.CommandHandlers.Operations
{
    public class DeleteOperationsHandler : BaseHandler<DeleteOperationsCommand>
    {
        public DeleteOperationsHandler(PiggyContext context, DeleteOperationsCommand command)
            : base(context, command)
        {
        }

        public override async Task Invoke(CancellationToken token)
        {
            var deletionService = new DeleteOperationService((PiggyContext)Context);
            var repository = GetRepository<Operation>();

            var ids = Command.Ids;
            foreach (var operation in repository.Where(o => !o.IsDeleted && ids.Contains(o.Id)).Select(o => new {o.Id, o.Type}))
            {
                switch (operation.Type)
                {
                    case OperationType.Budget:
                        await deletionService.DeleteBudgetOperation(operation.Id, Command, token);
                        break;
                    case OperationType.Transfer:
                        await deletionService.DeleteTransferOperation(operation.Id, Command, token);
                        break;
                }
            }
        }
    }
}