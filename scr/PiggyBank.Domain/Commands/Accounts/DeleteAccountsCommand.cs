namespace PiggyBank.Common.Commands.Accounts
{
    public class DeleteAccountsCommand : BaseModifiedCommand
    {
        public int[] Ids { get; set; }
    }
}