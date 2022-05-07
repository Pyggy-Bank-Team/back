using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PiggyBank.Common.Commands.Operations.Budget;
using PiggyBank.Common.Enums;
using PiggyBank.Domain.Handlers.Operations.Budget;
using PiggyBank.Domain.Models.Operations;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using Xunit;

namespace Domain.Tests.Handlers.Operations.Budget
{
    public class UpdateBudgetOperationHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;
        public UpdateBudgetOperationHandlerTest()
            => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "UpdateBudgetOperation_InMemory").Options);

        [Fact]
        public async Task Invoke_CommandHasAmountAndComment_OperationSuccessful()
        {
            var command = new UpdateBidgetOperationCommand
            {
                Id = 1,
                Amount = 100,
                Comment = "Test",
                AccountId = 1,
                CategoryId = 1,
                OperationDate = DateTime.UtcNow.AddDays(-2)
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

            var handler = new UpdateBudgetOperationHandler(_context, command);
            await handler.Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var operation = _context.BudgetOperations.First();

            Assert.Equal(command.Amount, operation.Amount);
            Assert.Equal(command.Comment, operation.Comment);
            Assert.Equal(command.OperationDate, operation.OperationDate);

            var account = _context.Accounts.First();
            var expectedAmount = 100;
            
            Assert.Equal(expectedAmount, account.Balance);
        }

        [Fact]
        public async void Invoke_NotRightOperationId_ThrowException()
        {
            var command = new UpdateBidgetOperationCommand
            {
                Amount = 100,
                Comment = "Test",
                AccountId = 2,
                CategoryId = 2
            };

            await _context.BudgetOperations.AddAsync(new BudgetOperation
            {
                Id = 1,
                Amount = 10,
                AccountId = 2,
                Comment = "Old new",
                CategoryId = 2
            });

            await _context.SaveChangesAsync();

            var handler = new UpdateBudgetOperationHandler(_context, command);
            var exception = await Assert.ThrowsAsync<ArgumentException>( () => handler.Invoke(CancellationToken.None));
            Assert.NotNull(exception);
        }
        
        [Fact]
        public async void Invoke_NotRightAccountId_ThrowException()
        {
            var command = new UpdateBidgetOperationCommand
            {
                Id = 1,
                Amount = 100,
                Comment = "Test",
                AccountId = 3
            };

            await _context.BudgetOperations.AddAsync(new BudgetOperation
            {
                Id = 1,
                Amount = 10,
                AccountId = 3,
                Comment = "Old new",
                Account = new Account
                {
                    Id = 3,
                    Balance = 100
                }
            });

            await _context.SaveChangesAsync();

            var handler = new UpdateBudgetOperationHandler(_context, command);
            var exception = await Assert.ThrowsAsync<ArgumentException>( () => handler.Invoke(CancellationToken.None));
            Assert.NotNull(exception);
        }
        
        [Fact]
        public async void Invoke_NotRightCategoryId_ThrowException()
        {
            var command = new UpdateBidgetOperationCommand
            {
                Id = 1,
                Amount = 100,
                Comment = "Test",
                AccountId = 4,
                CategoryId = 5
            };

            await _context.BudgetOperations.AddAsync(new BudgetOperation
            {
                Id = 1,
                Amount = 10,
                AccountId = 4,
                CategoryId = 4,
                Comment = "Old new",
                Account = new Account
                {
                    Id = 4,
                    Balance = 100
                },
                Category = new Category
                {
                    Id = 4,
                    Type = CategoryType.Expense
                }
            });

            await _context.SaveChangesAsync();

            var handler = new UpdateBudgetOperationHandler(_context, command);
            var exception = await Assert.ThrowsAsync<ArgumentException>( () => handler.Invoke(CancellationToken.None));
            Assert.NotNull(exception);
        }

        public void Dispose()
        {
            _context.BudgetOperations.RemoveRange(_context.BudgetOperations);
            _context.SaveChanges();
        }
    }
}
