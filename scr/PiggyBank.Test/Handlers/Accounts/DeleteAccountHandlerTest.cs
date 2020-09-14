using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Domain.Handler.Accounts;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using Xunit;

namespace PiggyBank.Test.Handlers.Accounts
{
    public class DeleteAccountHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;

        public DeleteAccountHandlerTest()
            => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAccountHandler_InMemory").Options);


        [Fact]
        public async Task Invoke_Default_DeletedAccount()
        {
            var account = new Account
            {
                Id = 1,
                IsDeleted = false,
                IsArchived = true
            };

            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            var handler = new DeleteAccountHandler(_context, 1);
            await handler.Invoke(CancellationToken.None);

            var deletedAccount = _context.Accounts.First();
            Assert.True(deletedAccount.IsDeleted);
        }

        public void Dispose()
            => _context?.Dispose();
    }
}