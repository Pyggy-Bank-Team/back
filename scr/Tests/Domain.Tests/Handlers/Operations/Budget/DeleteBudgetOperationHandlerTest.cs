using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PiggyBank.Common.Commands.Operations.Budget;
using PiggyBank.Common.Enums;
using PiggyBank.Domain.Handler.Operations.Budget;
using PiggyBank.Domain.Models.Operations;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using Xunit;

namespace Domain.Tests.Handlers.Operations.Budget
{
    public class DeleteBudgetOperationHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;
        public DeleteBudgetOperationHandlerTest()
            => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "DeleteBudgetOperation_InMemory").Options);

        [Theory]
        [InlineData(100, -100, CategoryType.Income)]
        [InlineData(200, 200, CategoryType.Expense)]
        public async Task Invoke_ByDefault(decimal amount, decimal resultBalance, CategoryType categoryType)
        {
            await _context.Accounts.AddAsync(new Account { Id = 1 });
            await _context.BudgetOperations.AddAsync(new BudgetOperation
            {
                Id = 1,
                AccountId = 1,
                Amount = amount,
                CategoryId = 1,
                IsDeleted = false,
                Snapshot = JsonConvert.SerializeObject(new OperationSnapshot { CategoryType = categoryType })
            });
            await _context.SaveChangesAsync();

            var command = new DeleteBudgetOperationCommand
            {
                Id = 1,
                ModifiedBy = Guid.NewGuid(),
                ModifiedOn = DateTime.UtcNow
            };
            await new DeleteBudgetOperationHandler(_context, command).Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var operation = await _context.BudgetOperations.FirstAsync();
            var account = await _context.Accounts.FirstAsync();

            Assert.True(operation.IsDeleted);
            Assert.Equal(command.ModifiedBy, operation.ModifiedBy);
            Assert.Equal(command.ModifiedOn, operation.ModifiedOn);
            Assert.Equal(resultBalance, account.Balance);
        }

        [Fact]
        public async Task Invoke_AccountIsDeleted_BalanceNotChanged()
        {
            await _context.Accounts.AddAsync(new Account { Id = 1, IsDeleted = true, Balance = 100 });
            await _context.BudgetOperations.AddAsync(new BudgetOperation
            {
                Id = 1,
                AccountId = 1,
                Amount = 100,
                CategoryId = 1,
                IsDeleted = false,
                Snapshot = JsonConvert.SerializeObject(new OperationSnapshot { CategoryType = CategoryType.Income })
            });
            await _context.SaveChangesAsync();

            var command = new DeleteBudgetOperationCommand
            {
                Id = 1,
                ModifiedBy = Guid.NewGuid(),
                ModifiedOn = DateTime.UtcNow
            };
            await new DeleteBudgetOperationHandler(_context, command).Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var operation = await _context.BudgetOperations.FirstAsync();
            var account = await _context.Accounts.FirstAsync();

            Assert.True(operation.IsDeleted);
            Assert.Equal(100, account.Balance);
        }

        [Fact]
        public async Task Invoke_AccountIsArchived_BalanceNotChanged()
        {
            await _context.Accounts.AddAsync(new Account { Id = 1, IsArchived = true, Balance = 100 });
            await _context.BudgetOperations.AddAsync(new BudgetOperation
            {
                Id = 1,
                AccountId = 1,
                Amount = 100,
                CategoryId = 1,
                IsDeleted = false,
                Snapshot = JsonConvert.SerializeObject(new OperationSnapshot { CategoryType = CategoryType.Income })
            });
            await _context.SaveChangesAsync();

            var command = new DeleteBudgetOperationCommand
            {
                Id = 1,
                ModifiedBy = Guid.NewGuid(),
                ModifiedOn = DateTime.UtcNow
            };
            await new DeleteBudgetOperationHandler(_context, command).Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var operation = await _context.BudgetOperations.FirstAsync();
            var account = await _context.Accounts.FirstAsync();

            Assert.True(operation.IsDeleted);
            Assert.Equal(100, account.Balance);
        }

        public void Dispose()
        {
            _context.BudgetOperations.RemoveRange(_context.BudgetOperations);
            _context.Accounts.RemoveRange(_context.Accounts);
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
