using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Categories;
using PiggyBank.Domain.Handler.Categories;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PiggyBank.Test.Handlers.Categories
{
    public class PartialUpdateCategoryHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;
        public PartialUpdateCategoryHandlerTest()
            => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "PartialUpdateCategory_InMemory").Options);

        [Fact]
        public async Task Invoke_CommandHasTitle_OperationSuccessfull()
        {
            var command = new PartialUpdateCategoryCommand
            {
                Id = 1,
                Title = "New title"
            };

            _context.Categories.Add(new Category
            {
                Id = 1,
                Type = Common.Enums.CategoryType.Income,
                Title = "My test",
                HexColor = "#000000"
            });

            _context.SaveChanges();

            var handler = new PartialUpdateCategoryHandler(_context, command);
            await handler.Invoke(CancellationToken.None);

            var category = _context.Categories.First();

            Assert.Equal(command.Title, category.Title);
            Assert.NotEqual(command.Type, category.Type);
            Assert.NotEqual(command.HexColor, category.HexColor);
        }

        [Fact]
        public async Task Invoke_CommandHasType_OperationSuccessfull()
        {
            var command = new PartialUpdateCategoryCommand
            {
                Id = 1,
                Type = Common.Enums.CategoryType.Expense
            };

            _context.Categories.Add(new Category
            {
                Id = 1,
                Type = Common.Enums.CategoryType.Income,
                Title = "My test",
                HexColor = "#000000"
            });

            _context.SaveChanges();

            var handler = new PartialUpdateCategoryHandler(_context, command);
            await handler.Invoke(CancellationToken.None);

            var category = _context.Categories.First();

            Assert.Equal(command.Type, category.Type);
            Assert.NotEqual(command.Title, category.Title);
            Assert.NotEqual(command.HexColor, category.HexColor);
        }

        [Fact]
        public async Task Invoke_ToArchived_OperationSuccessfull()
        {
            var command = new PartialUpdateCategoryCommand
            {
                Id = 1,
                IsArchived = true
            };

            _context.Categories.Add(new Category
            {
                Id = 1,
                Type = Common.Enums.CategoryType.Income,
                Title = "My test",
                HexColor = "#000000",
                IsArchived = false
            });

            _context.SaveChanges();

            var handler = new PartialUpdateCategoryHandler(_context, command);
            await handler.Invoke(CancellationToken.None);

            var category = _context.Categories.First();

            Assert.Equal(command.IsArchived, category.IsArchived);
            Assert.NotEqual(command.Title, category.Title);
            Assert.NotEqual(command.HexColor, category.HexColor);
        }

        [Fact]
        public async Task Invoke_ReturnNull_OperationDeclined()
        {
            var command = new PartialUpdateCategoryCommand
            {
                Id = 1,
                IsArchived = true
            };

            _context.Categories.Add(new Category
            {
                Id = 2,
                Type = Common.Enums.CategoryType.Income,
                Title = "My test",
                HexColor = "#000000",
                IsArchived = false
            });

            _context.SaveChanges();

            var handler = new PartialUpdateCategoryHandler(_context, command);
            await handler.Invoke(CancellationToken.None);

            var category = _context.Categories.First();

            Assert.False(category.IsArchived);
        }

        public void Dispose()
        {
            _context.RemoveRange(_context.Categories);
            _context.SaveChanges();
        }
    }
}
