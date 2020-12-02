using System.Collections.Generic;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Common.Commands.Categories;

namespace PiggyBank.WebApi.Interfaces
{
    public interface IItemFactory
    {
        IEnumerable<AccountItem> GeAccountItems(string locale, string currency);
        IEnumerable<CategoryItem> GetCategoryItems(string locale);
    }
}