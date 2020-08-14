using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Operations;
using PiggyBank.Domain.Handler.Operations.Budget;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using Xunit;

namespace PiggyBank.Test.Handlers.Operations.Budget
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
                Comment = "Test"
            };

            await _context.BudgetOperations.AddAsync(new BudgetOperation
            {
                Id = 1,
                Amount = 10,
                Comment = "Old new"
            });

            await _context.SaveChangesAsync();

            var handler = new UpdateBudgetOperationHandler(_context, command);
            await handler.Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var operation = _context.BudgetOperations.First();

            Assert.Equal(command.Amount, operation.Amount);
            Assert.Equal(command.Comment, operation.Comment);
        }

        public void Dispose()
        {
            _context.BudgetOperations.RemoveRange(_context.BudgetOperations);
            _context.SaveChanges();
        }
    }
}
