// using System;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using PiggyBank.Common.Commands.Accounts;
// using PiggyBank.Domain.Handlers.Accounts;
// using PiggyBank.Model;
// using PiggyBank.Model.Models.Entities;
// using Xunit;
//
// namespace Domain.Tests.Handlers.Accounts
// {
//     public class ArchiveAccountHandlerTest : IDisposable
//     {
//         private readonly PiggyContext _context;
//
//         public ArchiveAccountHandlerTest()
//             => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
//                 .UseInMemoryDatabase(databaseName: "ArchiveAccountHandler_InMemory").Options);
//
//         [Fact]
//         public async Task Invoke_Default_ArchivedAccount()
//         {
//             var account = new Account
//             {
//                 Id = 1,
//                 IsDeleted = false
//             };
//
//             await _context.Accounts.AddAsync(account);
//             await _context.SaveChangesAsync();
//
//             var command = new ArchiveAccountCommand
//             {
//                 Id = 1,
//                 ModifiedBy = Guid.NewGuid(),
//                 ModifiedOn = DateTime.Now
//             };
//             
//             var handler = new ArchiveAccountHandler(_context, command);
//             await handler.Invoke(CancellationToken.None);
//             await _context.SaveChangesAsync();
//
//             var archivedAccount = _context.Accounts.First();
//             Assert.True(archivedAccount.IsArchived);
//             Assert.Equal(command.ModifiedBy, account.ModifiedBy);
//             Assert.Equal(command.ModifiedOn, command.ModifiedOn);
//         }
//         
//         [Fact]
//         public async Task Invoke_DeletedAccount_AccountNonArchived()
//         {
//             var account = new Account
//             {
//                 Id = 1,
//                 IsDeleted = true
//             };
//
//             await _context.Accounts.AddAsync(account);
//             await _context.SaveChangesAsync();
//             
//             var command = new ArchiveAccountCommand
//             {
//                 Id = 1,
//                 ModifiedBy = Guid.NewGuid(),
//                 ModifiedOn = DateTime.Now
//             };
//             
//             var handler = new ArchiveAccountHandler(_context, command);
//             await handler.Invoke(CancellationToken.None);
//             await _context.SaveChangesAsync();
//
//             var archivedAccount = _context.Accounts.First();
//             Assert.False(archivedAccount.IsArchived);
//         }
//
//         public void Dispose()
//         {
//             _context?.Accounts.RemoveRange(_context.Accounts);
//             _context?.SaveChanges();
//             _context?.Dispose();
//         }
//     }
// }