// using System;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using PiggyBank.Common.Commands.Categories;
// using PiggyBank.Common.Enums;
// using PiggyBank.Domain.Handlers.Categories;
// using PiggyBank.Model;
// using PiggyBank.Model.Models.Entities;
// using Xunit;
//
// namespace Domain.Tests.Handlers.Categories
// {
//     public class UpdateCategoryHandlerTest : IDisposable
//     {
//         private readonly PiggyContext _context;
//
//         public UpdateCategoryHandlerTest()
//             => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
//                 .UseInMemoryDatabase(databaseName: "UpdateCategoryHandler_InMemory").Options);
//
//         [Fact]
//         public async Task Invoke_Default_UpdatedCategory()
//         {
//             var command = new UpdateCategoryCommand
//             {
//                 Id = 1,
//                 Title = "New title",
//                 HexColor = "#ffffff",
//                 IsArchived = true
//             };
//
//             await _context.Categories.AddAsync(new Category
//             {
//                 Id = 1,
//                 Title = "title",
//                 Type = CategoryType.Income,
//                 HexColor = "#000000",
//                 IsArchived = false
//             });
//             await _context.SaveChangesAsync();
//             
//             var handler = new UpdateCategoryHandler(_context, command);
//             await handler.Invoke(CancellationToken.None);
//
//             var category = _context.Categories.First();
//             Assert.Equal(command.Title, category.Title);
//             Assert.Equal(command.HexColor, category.HexColor);
//             Assert.Equal(command.IsArchived, category.IsArchived);
//         } 
//
//         public void Dispose()
//             => _context?.Dispose();
//     }
// }