using System.ComponentModel;

namespace Common.Enums
{
    public enum AccountType
    {
        [Description("Undefined")]
        Undefined = 0,

        [Description("Cash")]
        Cash,

        [Description("Card")]
        Card
    }
}
