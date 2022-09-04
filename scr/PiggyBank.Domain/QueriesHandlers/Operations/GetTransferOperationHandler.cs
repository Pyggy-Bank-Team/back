using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Queries;
using Common.Results.Models.Dto.Operations;
using Common.Results.Operations.Transfer;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.QueriesHandlers.Operations
{
    public class GetTransferOperationHandler : IRequestHandler<GetTransferOperationQuery, GetTransferOperationResult>
    {
        private readonly ITransferOperationRepository _repository;

        public GetTransferOperationHandler(ITransferOperationRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetTransferOperationResult> Handle(GetTransferOperationQuery request, CancellationToken cancellationToken)
        {
            var operation = await _repository.GetAsync(request.UserId, request.OperationId, cancellationToken);

            if (operation == null)
                return new GetTransferOperationResult { ErrorCode = ErrorCodes.NotFound };

            return new GetTransferOperationResult
            {
                Data = new TransferDto
                {
                    Amount = operation.Amount,
                    Comment = operation.Comment,
                    Date = operation.OperationDate,
                    Id = operation.Id,
                    Type = operation.Type,
                    FromId = operation.From,
                    ToId = operation.To
                }
            };
        }
    }
}