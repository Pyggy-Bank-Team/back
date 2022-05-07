using PiggyBank.Common.Models.Dto;

namespace PiggyBank.Domain.Results.Accounts
{
    public class AddAccountResult : BaseResult
    {
        public AccountDto Data { get; set; }
    }
}