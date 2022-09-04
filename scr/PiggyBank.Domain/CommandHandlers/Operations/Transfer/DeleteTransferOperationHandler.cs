using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Commands.Accounts;
using Common.Commands.Operations.Transfer;
using Common.Results.Operations.Transfer;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.CommandHandlers.Operations.Transfer
{
    public class DeleteTransferOperationHandler : IRequestHandler<DeleteTransferOperationCommand, DeleteTransferOperationResult>
    {
        private readonly ITransferOperationRepository _repository;
        private readonly IMediator _mediator;

        public DeleteTransferOperationHandler(ITransferOperationRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }
        
        public async Task<DeleteTransferOperationResult> Handle(DeleteTransferOperationCommand request, CancellationToken cancellationToken)
        {
            var operation = await _repository.GetAsync(request.ModifiedBy, request.Id, cancellationToken);

            if (operation == null)
                return new DeleteTransferOperationResult { ErrorCode = ErrorCodes.NotFound };

            if (operation.IsDeleted)
                return new DeleteTransferOperationResult();

            operation.IsDeleted = true;
            operation.ModifiedBy = request.ModifiedBy;
            operation.ModifiedOn = request.ModifiedOn;

            _ = await _repository.UpdateAsync(operation, cancellationToken);

            _ = await _mediator.Send(new ChangeBalanceCommand
            {
                Amount = operation.Amount,
                AccountId = operation.From,
                UserId = request.ModifiedBy
            }, cancellationToken);
            
            _ = await _mediator.Send(new ChangeBalanceCommand
            {
                Amount = -operation.Amount,
                AccountId = operation.To,
                UserId = request.ModifiedBy
            }, cancellationToken);

            return new DeleteTransferOperationResult();
        }
    }
}
