using MediatR;
using PiggyBank.Common.Commands;
using PiggyBank.Domain.Results.Accounts;

namespace PiggyBank.Domain.Commands.Accounts
{
    public class ArchiveAccountCommand : BaseModifiedCommand, IRequest<ArchiveAccountResult>
    {
        public int Id { get; set; }
    }
}