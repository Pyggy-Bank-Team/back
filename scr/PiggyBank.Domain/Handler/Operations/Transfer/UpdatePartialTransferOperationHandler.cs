using System;
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
            : base(context, command)
        {
        }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<TransferOperation>();
            var operation = await repository.FirstOrDefaultAsync(o => o.Id == Command.Id, token);

            if (operation == null)
            {
                //TODO: Add a log
                throw new ArgumentException($"Can't find operation with {Command.Id}");
            }

            //If the amount was changed then I'm undo the last change and 
            //Confirm the current amount
            if (Command.Amount.HasValue && operation.Amount != Command.Amount)
            {
                var fromAccount = await GetRepository<Account>().FirstOrDefaultAsync(a => !a.IsArchived && a.Id == Command.From, token)
                                  ?? await GetRepository<Account>().FirstOrDefaultAsync(a => !a.IsArchived && a.Id == operation.From, token);
                var toAccount = await GetRepository<Account>().FirstOrDefaultAsync(c => !c.IsArchived && c.Id == Command.To, token)
                                ?? await GetRepository<Account>().FirstOrDefaultAsync(a => !a.IsArchived && a.Id == operation.Id, token);
                fromAccount.ChangeBalance(operation.Amount);
                toAccount.ChangeBalance(-operation.Amount);

                fromAccount.ChangeBalance(-Command.Amount.Value);
                toAccount.ChangeBalance(Command.Amount.Value);

                GetRepository<Account>().Update(fromAccount);
                GetRepository<Account>().Update(toAccount);
            }

            operation.Amount = Command.Amount ?? operation.Amount;
            operation.Comment = string.IsNullOrWhiteSpace(Command.Comment) ? operation.Comment : Command.Comment;
            operation.From = Command.From ?? operation.From;
            operation.To = Command.To ?? operation.To;
            operation.ModifiedBy = Command.ModifiedBy;
            operation.ModifiedOn = Command.ModifiedOn;
            operation.OperationDate = Command.OperationDate ?? operation.OperationDate;

            repository.Update(operation);
        }
    }
}