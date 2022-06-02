using System.Collections.Generic;
using PiggyBank.Common.Commands.Categories;

namespace PiggyBank.WebApi.Interfaces
{
    public interface IItemFactory
    {
        IEnumerable<CategoryItem> GetCategoryItems(string locale);
    }
}