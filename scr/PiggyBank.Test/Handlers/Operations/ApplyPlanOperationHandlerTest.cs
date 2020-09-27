using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Enums;
using PiggyBank.Domain.Handler.Operations;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Operations.Plan;
using PiggyBank.Domain.Handler.Operations.Plan;
using Xunit;

namespace PiggyBank.Test.Handlers.Operations
{
    public class ApplyPlanOperationHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;

        public ApplyPlanOperationHandlerTest()
        {
            var options = new DbContextOptionsBuilder<PiggyContext>().EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "ApplyPlanOperation_InMemory").Options;
            _context = new PiggyContext(options);
        }

        [Fact]
        public async Task Invoke_Default_AppliedPlanOperation()
        {
            await _context.Accounts.AddAsync(new Account
            {
                Id = 1,
                Balance = 100
            });

            await _context.Categories.AddAsync(new Category
            {
                Id = 1,
                Type = CategoryType.Income
            });
            
            await _context.PlanOperations.AddAsync(new PlanOperation
            {
                Id = 2,
                IsDeleted = false,
                Amount = 100,
                CategoryId = 1,
                AccountId = 1,
                Comment = "Hello, world",
                CreatedBy = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                PlanDate = DateTime.Now.AddDays(2),
                Type = OperationType.Plan
            });
            await _context.SaveChangesAsync();

            var command = new ApplyPlanOperationCommand
            {
                Id = 2,
                ModifiedBy = Guid.NewGuid(),
                ModifiedOn = DateTime.UtcNow
            };

            await new ApplyPlanOperationHandler(_context, command).Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var planOperation = _context.PlanOperations.First(p => p.Id == 2);
            var budgetOperation = _context.BudgetOperations.First();

            Assert.True(planOperation.IsDeleted);
            Assert.Equal(planOperation.Amount, budgetOperation.Amount);
            Assert.Equal(planOperation.CategoryId, budgetOperation.CategoryId);
            Assert.Equal(planOperation.AccountId, budgetOperation.AccountId);
            Assert.Equal(planOperation.Comment, budgetOperation.Comment);
        }

        [Fact]
        public async Task Invoke_PlanOperationIdIsInvalid_ThrowsException()
        {
            var command = new ApplyPlanOperationCommand {Id = 1};
            var handler = new ApplyPlanOperationHandler(_context, command);
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Invoke(CancellationToken.None));
        }

        public void Dispose()
        {
            _context.PlanOperations.RemoveRange(_context.PlanOperations);
            _context.BudgetOperations.RemoveRange(_context.BudgetOperations);
            _context.Dispose();
        }
    }
}