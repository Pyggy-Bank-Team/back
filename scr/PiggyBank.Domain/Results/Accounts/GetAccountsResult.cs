using PiggyBank.Common.Models.Dto;

namespace PiggyBank.Domain.Results.Accounts
{
    public class GetAccountsResult : BaseResult
    {
        public AccountDto[] Data { get; set; }
    }
}