using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Operations.Transfer;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Operations.Transfer
{
    public class UpdatePartialTransferOperationHandler : BaseHandler<UpdatePartialTransferOperationCommand>
    {
        public UpdatePartialTransferOperationHandler(PiggyContext context, UpdatePartialTransferOperationCommand command) 
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<TransferOperation>();
            var operation = await repository.FirstOrDefaultAsync(o => o.Id == Command.Id, token);

            if (operation == null)
                return;

            operation.Amount = Command.Amount ?? operation.Amount;
            operation.Comment = string.IsNullOrWhiteSpace(Command.Comment) ? operation.Comment : Command.Comment;
            operation.From = Command.From ?? operation.From;
            operation.To = Command.To ?? operation.To;

            repository.Update(operation);
        }
    }
}