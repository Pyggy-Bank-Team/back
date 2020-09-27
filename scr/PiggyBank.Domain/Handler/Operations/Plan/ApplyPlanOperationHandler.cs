﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Operations.Plan;
using PiggyBank.Common.Enums;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Operations.Plan
{
    public class ApplyPlanOperationHandler : BaseHandler<ApplyPlanOperationCommand>
    {
        public ApplyPlanOperationHandler(PiggyContext context, ApplyPlanOperationCommand command)
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var budget = GetRepository<BudgetOperation>();
            var plan = GetRepository<PlanOperation>();
            var accounts = GetRepository<Account>();

            var operation = await plan.FirstOrDefaultAsync(p => p.Id == Command.Id, token)
            ?? throw new ArgumentException("Can't found plan operaton");

            operation.IsDeleted = true;
            operation.ModifiedBy = Command.ModifiedBy;
            operation.ModifiedOn = Command.ModifiedOn;
            plan.Update(operation);

            var account = await accounts.FirstOrDefaultAsync(a => a.Id == operation.AccountId && !a.IsDeleted, token)
                ?? throw new ArgumentException($"Can't found account by {operation.AccountId}");

            var category = await GetRepository<Category>().FirstOrDefaultAsync(c => c.Id == operation.CategoryId && !c.IsDeleted, token)
                ?? throw new ArgumentException($"Can't found category by {operation.CategoryId}");

            await budget.AddAsync(new BudgetOperation
            {
                AccountId = operation.AccountId,
                Amount = operation.Amount,
                CategoryId = operation.CategoryId,
                Comment = operation.Comment,
                CreatedBy = operation.CreatedBy,
                CreatedOn = operation.CreatedOn,
                Type = OperationType.Budget,
                Snapshot = operation.Snapshot
            }, token);

            account.ChangeBalance(category.Type == CategoryType.Income ? operation.Amount : -operation.Amount);

            accounts.Update(account);
        }
    }
}
