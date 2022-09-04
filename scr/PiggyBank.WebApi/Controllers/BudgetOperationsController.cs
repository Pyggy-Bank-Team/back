using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Operations.Budget;
using Common.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.WebApi.Extensions;
using PiggyBank.WebApi.Filters;
using PiggyBank.WebApi.Requests.Operations.Budget;
using PiggyBank.WebApi.Responses;

namespace PiggyBank.WebApi.Controllers
{
    [Authorize]
    [ApiController, Route("api/operations/budget")]
    public class BudgetOperationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BudgetOperationsController(IMediator mediator)
            => _mediator = mediator;

        [ValidateRequest, HttpPost]
        public async Task<IActionResult> CreateBudgetOperation(CreateBudgetOperationRequest request, CancellationToken token)
        {
            var command = new AddBudgetOperationCommand
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                CategoryId = request.CategoryId,
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
        public async Task<IActionResult> DeleteBudgetOperation(int operationId, CancellationToken token)
        {
            var command = new DeleteBudgetOperationCommand
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
        public async Task<IActionResult> UpdateBudgetOperation(int operationId, UpdateBudgetOperationRequest request, CancellationToken token)
        {
            var command = new UpdateBudgetOperationCommand
            {
                Id = operationId,
                AccountId = request.AccountId,
                CategoryId = request.CategoryId,
                Amount = request.Amount,
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
        public async Task<IActionResult> GetBudgetOperation(int operationId, CancellationToken token)
        {
            var query = new GetBudgetOperationQuery
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