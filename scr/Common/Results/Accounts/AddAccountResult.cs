using Common.Results.Models.Dto;

namespace Common.Results.Accounts
{
    public class AddAccountResult : BaseResult
    {
        public AccountDto Data { get; set; }
    }
}