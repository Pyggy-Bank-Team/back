using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PiggyBank.Common.Commands.Operations.Budget;
using PiggyBank.Common.Enums;
using PiggyBank.Common.Results.Models.Dto.Operations;
using PiggyBank.Domain.Models.Operations;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handlers.Operations.Budget
{
    public class AddBudgetOperationHandler : BaseHandler<AddBudgetOperationCommand>
    {
        public AddBudgetOperationHandler(PiggyContext context, AddBudgetOperationCommand command)
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var accountRepository = GetRepository<Account>();
            var account = await accountRepository.FirstOrDefaultAsync(a => a.Id == Command.AccountId && !a.IsDeleted, token)
                ?? throw new ArgumentException($"Can't found account by {Command.AccountId}");

            var category = await GetRepository<Category>().FirstOrDefaultAsync(c => c.Id == Command.CategoryId && !c.IsDeleted, token)
                ?? throw new ArgumentException($"Can't found category by {Command.CategoryId}");

            var snapshot = new OperationSnapshot
            {
                CategoryType = category.Type
            };

            var operation = new BudgetOperation
            {
                Amount = Command.Amount,
                Type = OperationType.Budget,
                Comment = Command.Comment,
                AccountId = Command.AccountId,
                CategoryId = Command.CategoryId,
                CreatedOn = Command.CreatedOn,
                Snapshot = JsonConvert.SerializeObject(snapshot),
                OperationDate = Command.OperationDate,
                CreatedBy = Command.CreatedBy
            };

            account.ChangeBalance(category.Type == CategoryType.Income ? operation.Amount : -operation.Amount);

            accountRepository.Update(account);

            var result = await GetRepository<BudgetOperation>().AddAsync(operation, token);
            await SaveAsync();

            var entity = result.Entity;
            Result = new BudgetDto
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Comment = entity.Comment,
                Date = entity.OperationDate,
                Type = entity.Type,
                AccountId = entity.AccountId,
                CategoryId = entity.CategoryId
            };
        }
    }
}
