﻿using System.Linq;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Accounts;

namespace PiggyBank.Domain.Handler.Accounts
{
    public class DeleteAccountHandler : BaseHandler<DeleteAccountCommand>
    {
        public DeleteAccountHandler(PiggyContext context, DeleteAccountCommand command)
            : base(context, command)
        {
        }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<Account>();
            var account = await repository.FirstOrDefaultAsync(a => a.Id == Command.Id && !a.IsDeleted, cancellationToken: token);

            if (account == null)
                return;

            account.IsDeleted = true;
            account.ModifiedBy = Command.ModifiedBy;
            account.ModifiedOn = Command.ModifiedOn;
            repository.Update(account);
            
            //Delete all related operations

            foreach (var budgetOperation in GetRepository<BudgetOperation>().Where(b => !b.IsDeleted && b.AccountId == account.Id))
            {
                budgetOperation.IsDeleted = true;
                GetRepository<BudgetOperation>().Update(budgetOperation);
            }
            
            foreach (var transferOperation in GetRepository<TransferOperation>().Where(b => !b.IsDeleted && (b.From == account.Id || b.To == account.Id)))
            {
                transferOperation.IsDeleted = true;
                GetRepository<TransferOperation>().Update(transferOperation);
            }
        }
    }
}