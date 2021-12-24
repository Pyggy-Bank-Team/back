using System;
using System.Collections.Generic;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Test.Subs
{
    public static class Operations
    {
        public static List<BudgetOperation> GetBudgetOperations()
        {
            var category = new Category
            {
                Id = 1,
                Title = "Test_1",
                HexColor = "#000000"
            };

            return new List<BudgetOperation>
            {
                new BudgetOperation
                {
                    Id = 1,
                    Amount = 10,
                    CategoryId = 1,
                    Category = category,
                    CreatedOn = new DateTime(2020, 10, 1),
                    CreatedBy = Guid.Empty
                },
                new BudgetOperation
                {
                    Id = 2,
                    Amount = 20,
                    CategoryId = 1,
                    Category = category,
                    CreatedOn = new DateTime(2020, 10, 7),
                    CreatedBy = Guid.Empty
                },
                new BudgetOperation
                {
                    Id = 3,
                    Amount = 10,
                    CategoryId = 2,
                    Category = new Category
                    {
                        Id = 2,
                        Title = "Test_2",
                        HexColor = "#FFFFFF"
                    },
                    CreatedOn = new DateTime(2020, 10, 5),
                    CreatedBy = Guid.Empty
                }
            };
        }
    }
}