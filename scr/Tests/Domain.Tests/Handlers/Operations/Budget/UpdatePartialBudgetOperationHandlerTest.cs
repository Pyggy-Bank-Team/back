using System;
using System.Linq;
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
    public class UpdatePartialBudgetOperationHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;

        public UpdatePartialBudgetOperationHandlerTest()
            => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "UpdatePartialBudgetOperation_InMemory").Options);

        [Fact]
        public async Task Invoke_CommandHasAmount_Successful()
        {
            var command = new UpdatePartialBidgetOperationCommand
            {
                Id = 1,
                Amount = 100
            };

            await _context.BudgetOperations.AddAsync(new BudgetOperation
            {
                Id = 1,
                Amount = 10,
                AccountId = 1,
                Comment = "Old new",
                CategoryId = 1,
                Category = new Category
                {
                    Id = 1,
                    Type = CategoryType.Income
                },
                Account = new Account
                {
                    Id = 1,
                    Balance = 100,
                    IsArchived = false
                },
                Snapshot = JsonConvert.SerializeObject(new OperationSnapshot{CategoryType = CategoryType.Income})
            });
            await _context.SaveChangesAsync();

            var handler = new UpdatePartialBudgetOperationHandler(_context, command);
            await handler.Invoke(CancellationToken.None);

            var operation = await _context.BudgetOperations.FirstAsync();
            
            Assert.Equal(command.Amount, operation.Amount);
            Assert.NotEqual(command.Comment, operation.Comment);
            Assert.NotEqual(command.AccountId, operation.AccountId);
            Assert.NotEqual(command.CategoryId, operation.CategoryId);

            var account = _context.Accounts.First();
            var exceptedAmount = 190;
            Assert.Equal(exceptedAmount, account.Balance);
        }
        
        [Fact]
        public async Task Invoke_CommandHasComment_Successful()
        {
            var command = new UpdatePartialBidgetOperationCommand
            {
                Id = 1,
                Comment = "New comment"
            };

            await _context.BudgetOperations.AddAsync(new BudgetOperation
            {
                Id = 1,
                Amount = 10,
                Comment = "Test comment",
                AccountId = 1,
                CategoryId = 1
            });

            await _context.SaveChangesAsync();

            var handler = new UpdatePartialBudgetOperationHandler(_context, command);
            await handler.Invoke(CancellationToken.None);

            var operation = await _context.BudgetOperations.FirstAsync();
            
            Assert.Equal(command.Comment, operation.Comment);
            Assert.NotEqual(command.Amount, operation.Amount);
            Assert.NotEqual(command.AccountId, operation.AccountId);
            Assert.NotEqual(command.CategoryId, operation.CategoryId);
        }
        
        [Fact]
        public async Task Invoke_CommandWithoutData_Unsuccessful()
        {
            var command = new UpdatePartialBidgetOperationCommand
            {
                Id = 1
            };

            await _context.BudgetOperations.AddAsync(new BudgetOperation
            {
                Id = 1,
                Amount = 10,
                Comment = "Test comment",
                AccountId = 1,
                CategoryId = 1
            });

            await _context.SaveChangesAsync();

            var handler = new UpdatePartialBudgetOperationHandler(_context, command);
            await handler.Invoke(CancellationToken.None);

            var operation = await _context.BudgetOperations.FirstAsync();
            
            Assert.NotEqual(command.Comment, operation.Comment);
            Assert.NotEqual(command.Amount, operation.Amount);
            Assert.NotEqual(command.AccountId, operation.AccountId);
            Assert.NotEqual(command.CategoryId, operation.CategoryId);
        }
        
        [Fact]
        public async Task Invoke_CommandHasOperationDate_Successful()
        {
            var command = new UpdatePartialBidgetOperationCommand
            {
                Id = 1,
                OperationDate = DateTime.UtcNow.AddDays(-2)
            };

            await _context.BudgetOperations.AddAsync(new BudgetOperation
            {
                Id = 1,
                Amount = 10,
                Comment = "Test comment",
                AccountId = 1,
                CategoryId = 1
            });

            await _context.SaveChangesAsync();

            var handler = new UpdatePartialBudgetOperationHandler(_context, command);
            await handler.Invoke(CancellationToken.None);

            var operation = await _context.BudgetOperations.FirstAsync();
            
            Assert.Equal(command.OperationDate, operation.OperationDate);
            Assert.NotEqual(command.Amount, operation.Amount);
            Assert.NotEqual(command.AccountId, operation.AccountId);
            Assert.NotEqual(command.CategoryId, operation.CategoryId);
        }

        public void Dispose()
        {
            _context.BudgetOperations.RemoveRange(_context.BudgetOperations);
            _context.SaveChanges();
        }
    }
}