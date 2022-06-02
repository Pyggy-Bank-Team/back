using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Enums;
using PiggyBank.Domain.Commands.Accounts;
using PiggyBank.Domain.Handlers.Accounts;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using Xunit;

namespace Domain.Tests.Handlers.Accounts
{
    public class UpdateAccountHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;

        public UpdateAccountHandlerTest()
            => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "UpdateAccountHandler_InMemory").Options);

        [Fact]
        public async Task Invoke_Default_UpdatedAccount()
        {
            var command = new UpdateAccountCommand
            {
                Id = 1,
                Balance = 10M,
                Currency = "USD",
                Title = "Test_1",
                Type = AccountType.Card,
                IsArchived = true
            };

            await _context.Accounts.AddAsync(new Account
            {
                Id = 1,
                Balance = 100M,
                Currency = "EUR",
                Title = "Test_0",
                Type = AccountType.Cash,
                IsArchived = false
            });
            await _context.SaveChangesAsync();
            
            var handler = new UpdateAccountHandler(_context, command);
            await handler.Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var account = _context.Accounts.First();
            Assert.Equal(command.Balance, account.Balance);
            Assert.Equal(command.Title, account.Title);
            Assert.Equal(command.Type, account.Type);
            Assert.Equal(command.IsArchived, account.IsArchived);
            Assert.NotEqual(command.Currency, account.Currency);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}