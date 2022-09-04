using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Operations.Transfer;
using Common.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.WebApi.Extensions;
using PiggyBank.WebApi.Filters;
using PiggyBank.WebApi.Requests.Operations.Transfer;
using PiggyBank.WebApi.Responses;

namespace PiggyBank.WebApi.Controllers
{
    [Authorize]
    [ApiController, Route("api/operations/transfer")]
    public class TransferOperationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransferOperationsController(IMediator mediator)
            => _mediator = mediator;

        [ValidateRequest, HttpPost]
        public async Task<IActionResult> PostTransfer(CreateTransferOperationRequest request, CancellationToken token)
        {
            var command = new AddTransferOperationCommand
            {
                Amount = request.Amount,
                From = request.From,
                To = request.To,
                Comment = request.Comment,
                OperationDate = request.OperationDate ?? DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = User.GetUserId()
            };

            var result = await _mediator.Send(command, token);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
        }

        [HttpDelete("{operationId}")]
        public async Task<IActionResult> DeleteTransferOperation(int operationId, CancellationToken token)
        {
            var command = new DeleteTransferOperationCommand
            {
                Id = operationId,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            var result = await _mediator.Send(command, token);

            if (result.IsSuccess)
                return Ok();

            return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
        }

        [ValidateRequest, HttpPut("{operationId}")]
        public async Task<IActionResult> UpdateTransferOperation(int operationId, UpdateTransferOperationRequest request, CancellationToken token)
        {
            var command = new UpdateTransferOperationCommand
            {
                Id = operationId,
                Amount = request.Amount,
                To = request.To,
                From = request.From,
                Comment = request.Comment,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow,
                OperationDate = request.OperationDate
            };

            var result = await _mediator.Send(command, token);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
        }

        [HttpGet("{operationId}")]
        public async Task<IActionResult> GetTransferOperation(int operationId, CancellationToken token)
        {
            var query = new GetTransferOperationQuery
            {
                OperationId = operationId,
                UserId = User.GetUserId()
            };

            var result = await _mediator.Send(query, token);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
        }
    }
}