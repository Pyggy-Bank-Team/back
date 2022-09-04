using Common.Results.Accounts;
using MediatR;

namespace Common.Commands.Accounts
{
    public class AddDefaultAccountsCommand : BaseCreateCommand, IRequest<AddDefaultAccountsResult>
    {
        public string Locale { get; set; }
        public string Currency { get; set; }
    }
}