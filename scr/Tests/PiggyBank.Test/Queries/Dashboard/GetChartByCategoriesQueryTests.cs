using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Reports;
using PiggyBank.Domain.Queries.Reports;
using PiggyBank.Model;
using PiggyBank.Test.Subs;
using Xunit;

namespace PiggyBank.Test.Queries.Dashboard
{
    public class GetChartByCategoriesQueryTests
    {
        private readonly PiggyContext _context;

        public GetChartByCategoriesQueryTests()
        {
            _context = new PiggyContext(new DbContextOptionsBuilder<PiggyContext>()
                .UseInMemoryDatabase(databaseName: "GetChartByCategoriesQueryTests")
                .Options);
        }

        [Fact(Skip = "With InMemoryDatabase doesn't work")]
        public async Task Invoke_Default_ReturnedChart()
        {
            await _context.BudgetOperations.AddRangeAsync(Operations.GetBudgetOperations());
            await _context.SaveChangesAsync();

            var command = new GetChartCommand
            {
                From = new DateTime(2020, 10, 1),
                To = new DateTime(2020, 10, 30),
                UserId = Guid.Empty
            };
            
            var result =  await new GetChartByCategoriesQuery(_context, command).Invoke(CancellationToken.None);
            
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal(30, result[0].Amount);
            Assert.Equal(10, result[1].Amount);
            Assert.Equal(1, result[0].CategoryId);
            Assert.Equal(2, result[1].CategoryId);
        }
    }
}