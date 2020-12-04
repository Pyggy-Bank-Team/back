using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PiggyBank.Common.Commands;
using PiggyBank.Common.Enums;
using PiggyBank.Domain.Models.Operations;
using PiggyBank.Model;

namespace PiggyBank.Domain.InternalServices
{
    internal class DeleteOperationService
    {
        private readonly PiggyContext _context;

        public DeleteOperationService(PiggyContext context)
        {
            _context = context;
        }

        public async Task DeleteBudgetOperation(int operationId, BaseModifiedCommand command, CancellationToken token)
        {
            var operation = await _context.BudgetOperations.FirstOrDefaultAsync(o => o.Id == operationId && !o.IsDeleted, token);

            if (operation == null)
                return;

            operation.IsDeleted = true;
            operation.ModifiedBy = command.ModifiedBy;
            operation.ModifiedOn = command.ModifiedOn;
            _context.BudgetOperations.Update(operation);

            var account = _context.Accounts.FirstOrDefault(a => a.Id == operation.AccountId
                                                                 && !a.IsDeleted && !a.IsArchived);

            if (account != null)
            {
                var snapshot = JsonConvert.DeserializeObject<OperationSnapshot>(operation.Snapshot);

                account.ChangeBalance(snapshot.CategoryType == CategoryType.Income ? -operation.Amount : operation.Amount);
                _context.Accounts.Update(account);
            }
        }
        
        public async Task DeleteTransferOperation(int operationId, BaseModifiedCommand command, CancellationToken token)
        {
            var operation = await _context.TransferOperations.FirstOrDefaultAsync(t => t.Id == operationId, token);

            if (operation != null)
            {
                var accountRepository = _context.Accounts;
                var fromAccount = await accountRepository.FirstOrDefaultAsync(a => a.Id == operation.From, token)
                                  ?? throw new ArgumentException($"Can't found account by {operation.From}");

                var toAccount = await accountRepository.FirstOrDefaultAsync(a => a.Id == operation.To, token)
                                ?? throw new ArgumentException($"Can't found account by {operation.To}");

                fromAccount.ChangeBalance(operation.Amount);
                toAccount.ChangeBalance(-operation.Amount);

                accountRepository.UpdateRange(new[] { fromAccount, toAccount });

                operation.IsDeleted = true;
                operation.ModifiedBy = command.ModifiedBy;
                operation.ModifiedOn = command.ModifiedOn;
                _context.TransferOperations.Update(operation);
            }
        }
    }
}