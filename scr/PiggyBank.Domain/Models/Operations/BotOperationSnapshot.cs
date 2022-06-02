using Common.Enums;

namespace PiggyBank.Domain.Models.Operations
{
    public class BotOperationSnapshot
    {
        public long ChatId { get; set; }
        public Step Step { get; set; }
        public string CreatedBy { get; set; }
        public OperationType Type { get; set; }
        public CategoryType? CategoryType { get; set; }
        public double? Amount { get; set; }
        public int? AccountId { get; set; }
        public int? CategoryId { get; set; }
        public int? ToAccountId { get; set; }
    }

    public enum Step
    {
        Zero,
        One,
        Two,
        Three
    }
}