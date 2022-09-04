using System.ComponentModel;

namespace Common.Enums
{
    public enum OperationType
    {
        [Description("Undefined")]
        Undefined = 0,

        [Description("Income and Expenses")]
        Budget = 1,

        [Description("Transfer")]
        Transfer = 2
    }
}
