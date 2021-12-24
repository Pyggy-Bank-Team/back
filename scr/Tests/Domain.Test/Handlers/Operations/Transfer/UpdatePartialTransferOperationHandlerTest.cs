using System;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Operations.Transfer;
using PiggyBank.Domain.Handler.Operations.Transfer;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using Xunit;

namespace Domain.Test.Handlers.Operations.Transfer
{
    public class UpdatePartialTransferOperationHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;

        public UpdatePartialTransferOperationHandlerTest()
            => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "UpdatePartialTransferOperation_InMemory").Options);

        [Fact]
        public async void Invoke_Default_OperationUpdate()
        {
            var command = new UpdatePartialTransferOperationCommand
            {
                Id = 1,
                From = 1,
                To = 2,
                Amount = 100,
                OperationDate = DateTime.UtcNow.AddDays(-2)
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

            var handler = new UpdatePartialTransferOperationHandler(_context, command);
            await handler.Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var operation = _context.TransferOperations.First();
            Assert.Equal(command.Amount, operation.Amount);

            var fromAccount = _context.Accounts.First(a => a.Id == command.From);
            Assert.Equal(110, fromAccount.Balance);

            var toAccount = _context.Accounts.First(a => a.Id == command.To);
            Assert.Equal(290, toAccount.Balance);
            
            Assert.Equal(command.OperationDate, operation.OperationDate);
        }
        
        [Fact]
        public async void Invoke_CommandHasComment_OperationUpdate()
        {
            var command = new UpdatePartialTransferOperationCommand
            {
                Id = 1,
                From = 1,
                To =2,
                Comment = "New comment"
            };

            await _context.TransferOperations.AddAsync(new TransferOperation
            {
                Id = 1,
                Amount = 10,
                From = 1,
                To = 2,
                Comment = "Old comment"
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

            var handler = new UpdatePartialTransferOperationHandler(_context, command);
            await handler.Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var operation = _context.TransferOperations.First();
            Assert.Equal(command.Comment, operation.Comment);

            var fromAccount = _context.Accounts.First(a => a.Id == command.From);
            Assert.Equal(200, fromAccount.Balance);

            var toAccount = _context.Accounts.First(a => a.Id == command.To);
            Assert.Equal(200, toAccount.Balance);
        }
        
        [Fact]
        public async void Invoke_CommandHasOperationDate_OperationUpdate()
        {
            var command = new UpdatePartialTransferOperationCommand
            {
                Id = 1,
                From = 1,
                To =2,
                OperationDate = DateTime.UtcNow.AddDays(-2)
            };

            await _context.TransferOperations.AddAsync(new TransferOperation
            {
                Id = 1,
                Amount = 10,
                From = 1,
                To = 2,
                Comment = "Old comment"
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

            var handler = new UpdatePartialTransferOperationHandler(_context, command);
            await handler.Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var operation = _context.TransferOperations.First();
            Assert.Equal(command.OperationDate, operation.OperationDate);

            var fromAccount = _context.Accounts.First(a => a.Id == command.From);
            Assert.Equal(200, fromAccount.Balance);

            var toAccount = _context.Accounts.First(a => a.Id == command.To);
            Assert.Equal(200, toAccount.Balance);
        }

        public void Dispose()
        {
            _context.Accounts.RemoveRange(_context.Accounts);
            _context.Categories.RemoveRange(_context.Categories);
            _context.Operations.RemoveRange(_context.Operations);
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}