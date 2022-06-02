using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Categories;
using PiggyBank.Common.Enums;
using PiggyBank.Common.Results.Models.Dto;
using PiggyBank.Domain.Handlers.Categories;
using PiggyBank.Model;
using Xunit;

namespace Domain.Tests.Handlers.Categories
{
    public class AddCategoryHandlerTest : IDisposable
    {
        private readonly PiggyContext _context;

        public AddCategoryHandlerTest()
            => _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "AddCategoryHandler_InMemory").Options);

        [Fact]
        public async Task Invoke_Default_CreatedCategory()
        {
            var command = new AddCategoryCommand
            {
                Title = "Test title",
                HexColor = "#000000",
                Type = CategoryType.Income,
                IsArchived = true
            };
            
            var handler = new AddCategoryHandler(_context, command);
            await handler.Invoke(CancellationToken.None);

            var category = (CategoryDto) handler.Result;
            
            Assert.Equal(command.Type, category.Type);
            Assert.Equal(command.Title, category.Title);
            Assert.Equal(command.HexColor, category.HexColor);
            Assert.Equal(command.IsArchived, category.IsArchived);
            Assert.False(category.IsDeleted);
        }

        public void Dispose()
            => _context?.Dispose();
    }
}