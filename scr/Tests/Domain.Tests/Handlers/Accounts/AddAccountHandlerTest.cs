using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Common.Enums;
using PiggyBank.Common.Models.Dto;
using PiggyBank.Domain.Handler.Accounts;
using PiggyBank.Model;
using Xunit;

namespace Domain.Tests.Handlers.Accounts
{
    public class AddAccountHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;

        public AddAccountHandlerTest()
            => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "AddAccountHandler_InMemory").Options);

        [Fact]
        public async Task Invoke_Default_AddedAccount()
        {
            var command = new AddAccountCommand
            {
                Balance = 100,
                Currency = "USD",
                Title = "Test title",
                Type = AccountType.Card,
                CreatedBy = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                IsArchived = true
            };

            var handler = new AddAccountHandler(_context, command);
            await handler.Invoke(CancellationToken.None);

            var account = (AccountDto) handler.Result;

            Assert.Equal(command.Balance, account.Balance);
            Assert.Equal(command.Currency, account.Currency);
            Assert.Equal(command.Type, account.Type);
            Assert.Equal(command.Title, account.Title);
            Assert.Equal(command.CreatedBy, account.CreatedBy);
            Assert.Equal(command.CreatedOn, account.CreatedOn);
            Assert.Equal(command.IsArchived, account.IsArchived);
            Assert.False(account.IsDeleted);
            Assert.NotEqual(default, account.Id);
        }

        public void Dispose()
            => _context?.Dispose();
    }
}