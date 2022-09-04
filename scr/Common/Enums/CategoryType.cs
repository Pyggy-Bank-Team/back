using System.ComponentModel;

namespace Common.Enums
{
    public enum CategoryType
    {
        [Description("Undefined")]
        Undefined = 0,

        [Description("Income")]
        Income,

        [Description("Expense")]
        Expense
    }
}
