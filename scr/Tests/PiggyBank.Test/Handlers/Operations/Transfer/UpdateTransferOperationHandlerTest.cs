using System;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Operations.Transfer;
using PiggyBank.Domain.Handler.Operations.Transfer;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using Xunit;

namespace PiggyBank.Test.Handlers.Operations.Transfer
{
    public class UpdateTransferOperationHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;

        public UpdateTransferOperationHandlerTest()
            => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "UpdateTransferOperation_InMemory").Options);

        [Fact]
        public async void Invoke_Default_OperationUpdate()
        {
            var command = new UpdateTransferOperationCommand
            {
                Id = 1,
                From = 1,
                To = 2,
                Amount = 100
            };

            await _context.TransferOperations.AddAsync(new TransferOperation
            {
                Id = 1,
                Amount = 10,
                From = 1,
                To = 2
            });

            await _context.Accounts.AddRangeAsync(new[]
            {
                new Account
                {
                    Id = 1,
                    Balance = 200
                },
                new Account
                {
                    Id = 2,
                    Balance = 200
                }
            });
            await _context.SaveChangesAsync();

            var handler = new UpdateTransferOperationHandler(_context, command);
            await handler.Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var operation = _context.TransferOperations.First();
            Assert.Equal(command.Amount, operation.Amount);

            var fromAccount = _context.Accounts.First(a => a.Id == command.From);
            Assert.Equal(110, fromAccount.Balance);

            var toAccount = _context.Accounts.First(a => a.Id == command.To);
            Assert.Equal(290, toAccount.Balance);
        }

        public void Dispose()
        {
            _context.Accounts.RemoveRange(_context.Accounts);
            _context.Operations.RemoveRange(_context.Operations);
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}