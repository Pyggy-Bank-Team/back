using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PiggyBank.Common.Commands.Operations.Plan;
using PiggyBank.Common.Enums;
using PiggyBank.Domain.Models.Operations;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Operations.Plan
{
    public class AddPlanOperationHandler : BaseHandler<AddPlanOperationCommand>
    {
        public AddPlanOperationHandler(PiggyContext context, AddPlanOperationCommand command)
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

            var operation = new PlanOperation
            {
                Amount = Command.Amount,
                Type = OperationType.Plan,
                Comment = Command.Comment,
                AccountId = Command.AccountId,
                CategoryId = Command.CategoryId,
                PlanDate = Command.PlanDate,
                CreatedOn = Command.CreatedOn,
                Shapshot = JsonConvert.SerializeObject(snapshot),
                CreatedBy = Command.CreatedBy
            };

            await GetRepository<PlanOperation>().AddAsync(operation, token);
        }
    }
}
