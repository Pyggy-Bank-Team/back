using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Categories;
using PiggyBank.Domain.Handler.Categories;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using Xunit;

namespace Domain.Test.Handlers.Categories
{
    public class ArchiveCategoryHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;

        public ArchiveCategoryHandlerTest()
            => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "ArchiveCategoryHandler_InMemory").Options);

        [Fact]
        public async Task Invoke_Default_ArchivedCategory()
        {
            await _context.Categories.AddAsync(new Category
            {
                Id = 1,
                IsArchived = false
            });
            await _context.SaveChangesAsync();

            var command = new ArchiveCategoryCommand
            {
                Id = 1,
                ModifiedBy = Guid.NewGuid(),
                ModifiedOn = DateTime.Now
            };
            
            var handler = new ArchiveCategoryHandler(_context, command);
            await handler.Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var category = _context.Categories.First();
            Assert.True(category.IsArchived);
            Assert.Equal(command.ModifiedBy, category.ModifiedBy);
            Assert.Equal(command.ModifiedOn, category.ModifiedOn);
        }
        
        [Fact]
        public async Task Invoke_ArchiveDeletedCategory_CategoryNonArchived()
        {
            await _context.Categories.AddAsync(new Category
            {
                Id = 1,
                IsArchived = false,
                IsDeleted = true
            });
            await _context.SaveChangesAsync();
            
            var command = new ArchiveCategoryCommand
            {
                Id = 1,
                ModifiedBy = Guid.NewGuid(),
                ModifiedOn = DateTime.Now
            };
            
            var handler = new ArchiveCategoryHandler(_context, command);
            await handler.Invoke(CancellationToken.None);
            await _context.SaveChangesAsync();

            var category = _context.Categories.First();
            Assert.False(category.IsArchived);
        }

        public void Dispose()
        {
            _context?.Categories.RemoveRange(_context.Categories);
            _context?.SaveChanges();
            _context?.Dispose();
        }
    }
}