using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Domain.Handler.Categories;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using Xunit;

namespace PiggyBank.Test.Handlers.Categories
{
    public class DeleteCategoryHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;

        public DeleteCategoryHandlerTest()
            => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "DeleteCategoryHandler_InMemory").Options);

        [Fact]
        public async Task Invoke_Default_DeletedCategory()
        {
            await _context.Categories.AddAsync(new Category {Id = 1});
            await _context.SaveChangesAsync();
            
            var handler = new DeleteCategoryHandler(_context, 1);
            await handler.Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var category = _context.Categories.First();
            Assert.True(category.IsDeleted);
        }

        public void Dispose()
            => _context?.Dispose();
    }
}