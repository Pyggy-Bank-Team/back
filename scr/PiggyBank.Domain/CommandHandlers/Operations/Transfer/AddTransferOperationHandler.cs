using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Accounts;
using Common.Commands.Operations.Transfer;
using Common.Enums;
using Common.Queries;
using Common.Results.Models.Dto.Operations;
using Common.Results.Operations.Transfer;
using MediatR;
using PiggyBank.Model.Interfaces;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.CommandHandlers.Operations.Transfer
{
    public class AddTransferOperationHandler : IRequestHandler<AddTransferOperationCommand, AddTransferOperationResult>
    {
        private readonly ITransferOperationRepository _repository;
        private readonly IMediator _mediator;

        public AddTransferOperationHandler(ITransferOperationRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<AddTransferOperationResult> Handle(AddTransferOperationCommand request, CancellationToken cancellationToken)
        {
            var getFromAccountResult = await _mediator.Send(new GetAccountQuery{AccountId = request.From, UserId = request.CreatedBy}, cancellationToken);

            if (!getFromAccountResult.IsSuccess)
                return new AddTransferOperationResult { ErrorCode = getFromAccountResult.ErrorCode, Messages = getFromAccountResult.Messages };

            var getToAccountResult = await _mediator.Send(new GetAccountQuery{AccountId = request.To, UserId = request.CreatedBy}, cancellationToken);

            if (!getToAccountResult.IsSuccess)
                return new AddTransferOperationResult { ErrorCode = getToAccountResult.ErrorCode, Messages = getToAccountResult.Messages};
            
            var changeFromAccountAmountResult = await _mediator.Send(new ChangeBalanceCommand{AccountId = getFromAccountResult.Data.Id, Amount = -request.Amount, UserId = request.CreatedBy}, cancellationToken);

            if (!changeFromAccountAmountResult.IsSuccess)
                return new AddTransferOperationResult { ErrorCode = changeFromAccountAmountResult.ErrorCode, Messages = changeFromAccountAmountResult.Messages };
            
            var changeToAccountAmountResult = await _mediator.Send(new ChangeBalanceCommand{AccountId = getToAccountResult.Data.Id, Amount = request.Amount, UserId = request.CreatedBy}, cancellationToken);

            if (!changeToAccountAmountResult.IsSuccess)
                return new AddTransferOperationResult { ErrorCode = changeToAccountAmountResult.ErrorCode, Messages = changeToAccountAmountResult.Messages };

            var operation = new TransferOperation
            {
                Amount = request.Amount,
                Comment = request.Comment,
                From = request.From,
                To = request.To,
                CreatedBy = request.CreatedBy,
                CreatedOn = request.CreatedOn,
                OperationDate = request.OperationDate,
                Type = OperationType.Transfer
            };

            var createdOperation = await _repository.AddAsync(operation, cancellationToken);
            return new AddTransferOperationResult
            {
                Data = new TransferDto
                {
                    Amount = createdOperation.Amount,
                    Comment = createdOperation.Comment,
                    Date = createdOperation.OperationDate,
                    Id = createdOperation.Id,
                    Type = createdOperation.Type,
                    FromId = createdOperation.From,
                    ToId = createdOperation.To
                }
            };
        }
    }
}
