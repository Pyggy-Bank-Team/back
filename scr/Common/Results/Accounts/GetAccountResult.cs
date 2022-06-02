using Common.Results.Models.Dto;

namespace Common.Results.Accounts
{
    public class GetAccountResult : BaseResult
    {
        public AccountDto Data { get; set; }
    }
}