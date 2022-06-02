using MediatR;
using Common.Results.Accounts;

namespace Common.Commands.Accounts
{
    public class ArchiveAccountCommand : BaseModifiedCommand, IRequest<ArchiveAccountResult>
    {
        public int Id { get; set; }
    }
}