using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Accounts;
using Common.Enums;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Domain.CommandHandlers.Accounts;
using PiggyBank.Model;
using PiggyBank.Model.Repositories;
using Xunit;

namespace Domain.Tests.Handlers.Accounts
{
    public class AddAccountHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;

        public AddAccountHandlerTest()
            => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "AddAccountHandler_InMemory").Options);

        [Theory]
        [InlineData(AccountType.Card, "USD")]
        [InlineData(AccountType.Cash, "EUR")]
        public async Task Handle_Default_AddedAccount(AccountType type, string currency)
        {
            var repository = new AccountRepository(_context);
            var handler = new AddAccountHandler(repository);
            
            var command = new AddAccountCommand
            {
                Balance = 100,
                Currency = currency,
                Title = "Test title",
                Type = type,
                CreatedBy = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                IsArchived = true
            };
            
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(command.Balance, result.Data.Balance);
            Assert.Equal(command.Currency, result.Data.Currency);
            Assert.Equal(command.Type, result.Data.Type);
            Assert.Equal(command.Title, result.Data.Title);
            Assert.Equal(command.CreatedBy, result.Data.CreatedBy);
            Assert.Equal(command.CreatedOn, result.Data.CreatedOn);
            Assert.Equal(command.IsArchived, result.Data.IsArchived);
            Assert.False(result.Data.IsDeleted);
            Assert.NotEqual(default, result.Data.Id);
        }

        public void Dispose()
            => _context?.Dispose();
    }
}