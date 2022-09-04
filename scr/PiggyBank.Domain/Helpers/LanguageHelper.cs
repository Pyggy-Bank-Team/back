namespace PiggyBank.Domain.Helpers
{
    public interface ILanguageHelper
    {
        bool UseRussianLanguage(string locale);
    }

    public class LanguageHelper : ILanguageHelper
    {
        public bool UseRussianLanguage(string locale)
        {
            if (string.IsNullOrWhiteSpace(locale))
                return false;

            var lower = locale.ToLowerInvariant();
            return lower.Contains("ru") || lower.Contains("kz") || lower.Contains("by") || lower.Contains("ua");
        }
    }
}