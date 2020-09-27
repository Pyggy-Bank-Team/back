using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Operations.Transfer;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Operations.Transfer
{
    public class DeleteTransferOperationHandler : BaseHandler<DeleteTransferOperationCommand>
    {
        public DeleteTransferOperationHandler(PiggyContext context, DeleteTransferOperationCommand command)
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var operation = await GetRepository<TransferOperation>().FirstOrDefaultAsync(t => t.Id == Command.Id, token);

            if (operation != null)
            {
                var accountRepository = GetRepository<Account>();
                var fromAccount = await accountRepository.FirstOrDefaultAsync(a => a.Id == operation.From, token)
                ?? throw new ArgumentException($"Can't found account by {operation.From}");

                var toAccount = await accountRepository.FirstOrDefaultAsync(a => a.Id == operation.To, token)
                    ?? throw new ArgumentException($"Can't found account by {operation.To}");

                fromAccount.ChangeBalance(operation.Amount);
                toAccount.ChangeBalance(-operation.Amount);

                accountRepository.UpdateRange(new[] { fromAccount, toAccount });

                operation.IsDeleted = true;
                operation.ModifiedBy = Command.ModifiedBy;
                operation.ModifiedOn = Command.ModifiedOn;
                GetRepository<TransferOperation>().Update(operation);
            }
        }
    }
}
