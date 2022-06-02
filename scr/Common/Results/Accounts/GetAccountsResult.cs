using Common.Results.Models.Dto;

namespace Common.Results.Accounts
{
    public class GetAccountsResult : BaseResult
    {
        public AccountDto[] Data { get; set; }
    }
}