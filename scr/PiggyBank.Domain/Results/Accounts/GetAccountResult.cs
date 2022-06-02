using PiggyBank.Common.Models.Dto;

namespace PiggyBank.Domain.Results.Accounts
{
    public class GetAccountResult : BaseResult
    {
        public AccountDto Data { get; set; }
    }
}