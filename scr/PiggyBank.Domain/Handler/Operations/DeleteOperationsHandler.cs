using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Operations;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handler.Operations
{
    public class DeleteOperationsHandler : BaseHandler<DeleteOperationsCommand>
    {
        public DeleteOperationsHandler(PiggyContext context, DeleteOperationsCommand command)
            : base(context, command)
        {
        }

        public override Task Invoke(CancellationToken token)
            => Task.Run(() =>
            {
                var repository = GetRepository<Operation>();

                var ids = Command.Ids;
                foreach (var operation in repository.Where(o => !o.IsDeleted && ids.Contains(o.Id)))
                {
                    operation.IsDeleted = true;
                    operation.ModifiedBy = Command.ModifiedBy;
                    operation.ModifiedOn = Command.ModifiedOn;
                    repository.Update(operation);
                }
            }, token);
    }
}