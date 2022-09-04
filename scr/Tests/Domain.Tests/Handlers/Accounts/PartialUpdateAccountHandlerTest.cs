// using Microsoft.EntityFrameworkCore;
// using PiggyBank.Model;
// using PiggyBank.Model.Models.Entities;
// using System;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using PiggyBank.Common.Commands.Accounts;
// using PiggyBank.Common.Enums;
// using PiggyBank.Domain.Handlers.Accounts;
// using Xunit;
//
// namespace Domain.Tests.Handlers.Accounts
// {
//     public class PartialUpdateAccountHandlerTest : IDisposable
//     {
//         private readonly PiggyContext _context;
//         public PartialUpdateAccountHandlerTest()
//             => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
//                 .UseInMemoryDatabase(databaseName: "PartialUpdateAccount_InMemory").Options);
//
//         [Fact]
//         public async Task Invoke_CommandHasBalance_OperationSuccessfull()
//         {
//             var command = new PartialUpdateAccountCommand
//             {
//                 Id = 1,
//                 Balance = 100
//             };
//
//             _context.Accounts.Add(new Account
//             {
//                 Id = 1,
//                 Balance = 10,
//                 Title = "My test",
//                 Currency = "USD"
//             });
//
//             _context.SaveChanges();
//
//             var handler = new PartialUpdateAccountHandler(_context, command);
//             await handler.Invoke(CancellationToken.None);
//
//             var account = _context.Accounts.First();
//
//             Assert.Equal(command.Balance, account.Balance);
//             Assert.NotEqual(command.Title, account.Title);
//             Assert.NotEqual(command.Currency, account.Currency);
//         }
//
//         [Fact]
//         public async Task Invoke_CommandHasType_OperationSuccessfull()
//         {
//             var command = new PartialUpdateAccountCommand
//             {
//                 Id = 1,
//                 Type = AccountType.Cash
//             };
//
//             _context.Accounts.Add(new Account
//             {
//                 Id = 1,
//                 Type = AccountType.Card,
//                 Title = "My test",
//                 Currency = "USD"
//             });
//
//             _context.SaveChanges();
//
//             var handler = new PartialUpdateAccountHandler(_context, command);
//             await handler.Invoke(CancellationToken.None);
//
//             var account = _context.Accounts.First();
//
//             Assert.Equal(command.Type, account.Type);
//             Assert.NotEqual(command.Title, account.Title);
//             Assert.NotEqual(command.Currency, account.Currency);
//         }
//
//         [Fact]
//         public async Task Invoke_ToArchived_OperationSuccessfull()
//         {
//             var command = new PartialUpdateAccountCommand
//             {
//                 Id = 1,
//                 IsArchive = true
//             };
//
//             _context.Accounts.Add(new Account
//             {
//                 Id = 1,
//                 Type = AccountType.Card,
//                 Title = "My test",
//                 Currency = "USD",
//                 IsArchived = false
//             });
//
//             _context.SaveChanges();
//
//             var handler = new PartialUpdateAccountHandler(_context, command);
//             await handler.Invoke(CancellationToken.None);
//
//             var account = _context.Accounts.First();
//
//             Assert.Equal(command.IsArchive, account.IsArchived);
//             Assert.NotEqual(command.Title, account.Title);
//             Assert.NotEqual(command.Currency, account.Currency);
//         }
//
//         [Fact]
//         public async Task Invoke_ReturnNull_OperationDeclined()
//         {
//             var command = new PartialUpdateAccountCommand
//             {
//                 Id = 1,
//                 IsArchive = true
//             };
//
//             _context.Accounts.Add(new Account
//             {
//                 Id = 2,
//                 Type = AccountType.Card,
//                 Title = "My test",
//                 Currency = "USD",
//                 IsArchived = false
//             });
//
//             _context.SaveChanges();
//
//             var handler = new PartialUpdateAccountHandler(_context, command);
//             await handler.Invoke(CancellationToken.None);
//
//             var account = _context.Accounts.First();
//
//             Assert.False(account.IsArchived);
//         }
//
//         public void Dispose()
//         {
//             _context.Accounts.RemoveRange(_context.Accounts);
//             _context.SaveChanges();
//         }
//     }
// }
