using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Operations.Transfer;
using Common.Results.Operations.Transfer;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.CommandHandlers.Operations.Transfer
{
    public class DeleteTransferOperationsByAccountIdHandler : IRequestHandler<DeleteTransferOperationsByAccountIdCommand, DeleteTransferOperationsByAccountIdResult>
    {
        private readonly ITransferOperationRepository _repository;
        private readonly IMediator _mediator;

        public DeleteTransferOperationsByAccountIdHandler(ITransferOperationRepository repository, IMediator mediator)
            => (_repository, _mediator) = (repository, mediator);

        public async Task<DeleteTransferOperationsByAccountIdResult> Handle(DeleteTransferOperationsByAccountIdCommand request, CancellationToken cancellationToken)
        {
            var allTransferOperations = await _repository.GetAllAsync(request.UserId, cancellationToken);
            
            foreach (var transferOperation in allTransferOperations.Where(to => to.From == request.AccountId))
            {
                var deleteTransferOperationCommand = new DeleteTransferOperationCommand
                {
                    Id = transferOperation.Id,
                    ModifiedBy = request.ModifiedBy,
                    ModifiedOn = request.ModifiedOn
                };

                _ = await _mediator.Send(deleteTransferOperationCommand, cancellationToken);
            }

            return new DeleteTransferOperationsByAccountIdResult();
        }
    }
}