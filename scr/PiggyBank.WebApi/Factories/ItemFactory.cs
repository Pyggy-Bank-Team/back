using System.Collections.Generic;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Common.Commands.Categories;
using PiggyBank.Common.Enums;
using PiggyBank.WebApi.Interfaces;

namespace PiggyBank.WebApi.Factories
{
    public class ItemFactory : IItemFactory
    {
        public IEnumerable<AccountItem> GeAccountItems(string locale, string currency)
        {
            if (IsRussianLanguage(locale))
            {
                yield return new AccountItem
                {
                    Title = "Наличные",
                    Balance = 100M,
                    Currency = currency,
                    Type = AccountType.Cash,
                    IsArchived = false
                };

                yield return new AccountItem
                {
                    Title = "Дебетовая карточка",
                    Balance = 100M,
                    Currency = currency,
                    Type = AccountType.Card,
                    IsArchived = false
                };
            }
            else
            {
                yield return new AccountItem
                {
                    Title = "Cash",
                    Balance = 100M,
                    Currency = currency,
                    Type = AccountType.Cash,
                    IsArchived = false
                };

                yield return new AccountItem
                {
                    Title = "Card",
                    Balance = 100M,
                    Currency = currency,
                    Type = AccountType.Card,
                    IsArchived = false
                };
            }
        }

        public IEnumerable<CategoryItem> GetCategoryItems(string locale)
        {
            if (IsRussianLanguage(locale))
            {
                yield return new CategoryItem
                {
                    Title = "Доход",
                    Type = CategoryType.Income,
                    HexColor = "#bf0077",
                    IsArchived = false
                };
                
                yield return new CategoryItem
                {
                    Title = "Расход",
                    Type = CategoryType.Expense,
                    HexColor = "#0078d7",
                    IsArchived = false
                };
            }
            else
            {
                yield return new CategoryItem
                {
                    Title = "Income",
                    Type = CategoryType.Income,
                    HexColor = "#bf0077",
                    IsArchived = false
                };
                
                yield return new CategoryItem
                {
                    Title = "Expense",
                    Type = CategoryType.Expense,
                    HexColor = "#0078d7",
                    IsArchived = false
                };
            }
        }

        private bool IsRussianLanguage(string locale)
        {
            if (string.IsNullOrWhiteSpace(locale))
                return false;
            
            var lower = locale.ToLowerInvariant();
            return lower.Contains("ru") || lower.Contains("kz") || lower.Contains("by") || lower.Contains("ua");
        }
    }
}