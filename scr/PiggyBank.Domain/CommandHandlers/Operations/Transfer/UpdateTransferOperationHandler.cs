using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Commands.Accounts;
using Common.Commands.Operations.Transfer;
using Common.Queries;
using Common.Results.Models.Dto.Operations;
using Common.Results.Operations.Transfer;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.CommandHandlers.Operations.Transfer
{
    public class UpdateTransferOperationHandler : IRequestHandler<UpdateTransferOperationCommand, UpdateTransferOperationResult>
    {
        private readonly ITransferOperationRepository _repository;
        private readonly IMediator _mediator;

        public UpdateTransferOperationHandler(ITransferOperationRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<UpdateTransferOperationResult> Handle(UpdateTransferOperationCommand request, CancellationToken cancellationToken)
        {
            var operation = await _repository.GetAsync(request.ModifiedBy, request.Id, cancellationToken);

            if (operation == null)
                return new UpdateTransferOperationResult { ErrorCode = ErrorCodes.NotFound };
            
            var getFromAccountResult = await _mediator.Send(new GetAccountQuery{AccountId = request.From, UserId = request.ModifiedBy}, cancellationToken);

            if (!getFromAccountResult.IsSuccess)
                return new UpdateTransferOperationResult { ErrorCode = getFromAccountResult.ErrorCode, Messages = getFromAccountResult.Messages };

            var getToAccountResult = await _mediator.Send(new GetAccountQuery{AccountId = request.To, UserId = request.ModifiedBy}, cancellationToken);

            if (!getToAccountResult.IsSuccess)
                return new UpdateTransferOperationResult { ErrorCode = getToAccountResult.ErrorCode, Messages = getToAccountResult.Messages};
            
            var changeFromAccountAmountResult = await _mediator.Send(new ChangeBalanceCommand{AccountId = operation.From, Amount = operation.Amount, UserId = request.ModifiedBy}, cancellationToken);

            if (!changeFromAccountAmountResult.IsSuccess)
                return new UpdateTransferOperationResult { ErrorCode = changeFromAccountAmountResult.ErrorCode, Messages = changeFromAccountAmountResult.Messages };
            
            var changeToAccountAmountResult = await _mediator.Send(new ChangeBalanceCommand{AccountId = operation.To, Amount = -operation.Amount, UserId = request.ModifiedBy}, cancellationToken);

            if (!changeToAccountAmountResult.IsSuccess)
                return new UpdateTransferOperationResult { ErrorCode = changeToAccountAmountResult.ErrorCode, Messages = changeToAccountAmountResult.Messages };
            
            changeFromAccountAmountResult = await _mediator.Send(new ChangeBalanceCommand{AccountId = getFromAccountResult.Data.Id, Amount = -request.Amount, UserId = request.ModifiedBy}, cancellationToken);

            if (!changeFromAccountAmountResult.IsSuccess)
                return new UpdateTransferOperationResult { ErrorCode = changeFromAccountAmountResult.ErrorCode, Messages = changeFromAccountAmountResult.Messages };
            
            changeToAccountAmountResult = await _mediator.Send(new ChangeBalanceCommand{AccountId = getToAccountResult.Data.Id, Amount = request.Amount, UserId = request.ModifiedBy}, cancellationToken);

            if (!changeToAccountAmountResult.IsSuccess)
                return new UpdateTransferOperationResult { ErrorCode = changeToAccountAmountResult.ErrorCode, Messages = changeToAccountAmountResult.Messages };

            operation.Amount = request.Amount;
            operation.From = request.From;
            operation.To = request.To;
            operation.Comment = request.Comment;
            operation.OperationDate = request.OperationDate ?? operation.OperationDate;
            operation.ModifiedBy = request.ModifiedBy;
            operation.ModifiedOn = request.ModifiedOn;

            var updatedOperation = await _repository.UpdateAsync(operation, cancellationToken);
            return new UpdateTransferOperationResult
            {
                Data = new TransferDto
                {
                    Amount = updatedOperation.Amount,
                    Comment = updatedOperation.Comment,
                    Date = updatedOperation.OperationDate,
                    Id = updatedOperation.Id,
                    Type = updatedOperation.Type,
                    FromId = updatedOperation.From,
                    ToId = updatedOperation.To
                }
            };
        }
    }
}