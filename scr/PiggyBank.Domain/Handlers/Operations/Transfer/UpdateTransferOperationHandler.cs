using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Operations.Transfer;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handlers.Operations.Transfer
{
    public class UpdateTransferOperationHandler : BaseHandler<UpdateTransferOperationCommand>
    {
        public UpdateTransferOperationHandler(PiggyContext context, UpdateTransferOperationCommand command) 
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<TransferOperation>();
            var operation = await repository.FirstOrDefaultAsync(o => o.Id == Command.Id, token);

            if (operation == null)
            {
                //TODO: Add a log
                throw new ArgumentException($"Can't find operation with {Command.Id}");
            }

            var fromAccount = await GetRepository<Account>().FirstOrDefaultAsync(a => !a.IsArchived && a.Id == Command.From, token);
            if (fromAccount == null)
            {
                //TODO: Add a log
                throw new ArgumentException($"Can't find from account with id {Command.From}");
            }

            var toAccount = await GetRepository<Account>().FirstOrDefaultAsync(c => !c.IsArchived && c.Id == Command.To, token);
            if (toAccount == null)
            {
                //TODO: Add a log
                throw new ArgumentException($"Can't find to account with id {Command.To}");
            }

            //If the amount was changed then I undo the last change and 
            //Confirm the current amount
            if (operation.Amount != Command.Amount)
            {
                fromAccount.ChangeBalance(operation.Amount);
                toAccount.ChangeBalance(-operation.Amount);
                
                fromAccount.ChangeBalance(-Command.Amount);
                toAccount.ChangeBalance(Command.Amount);

                GetRepository<Account>().Update(fromAccount);
                GetRepository<Account>().Update(toAccount);
            }

            operation.Amount = Command.Amount;
            operation.Comment = Command.Comment;
            operation.From = Command.From;
            operation.To = Command.To;
            operation.ModifiedBy = Command.ModifiedBy;
            operation.ModifiedOn = Command.ModifiedOn;
            operation.OperationDate = Command.OperationDate ?? operation.OperationDate;

            repository.Update(operation);
        }
    }
}