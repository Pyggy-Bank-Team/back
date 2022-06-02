using System.Collections.Generic;
using PiggyBank.Common.Commands.Categories;
using PiggyBank.Domain.Commands.Accounts;

namespace PiggyBank.WebApi.Interfaces
{
    public interface IItemFactory
    {
        IEnumerable<AccountItem> GeAccountItems(string locale, string currency);
        IEnumerable<CategoryItem> GetCategoryItems(string locale);
    }
}