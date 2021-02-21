using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.Common.Interfaces;
using PiggyBank.WebApi.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Operations;
using PiggyBank.Common.Commands.Operations.Budget;
using PiggyBank.Common.Commands.Operations.Transfer;
using PiggyBank.Common.Models;
using PiggyBank.Common.Models.Dto.Operations;
using PiggyBank.Common.Queries;
using PiggyBank.WebApi.Filters;
using PiggyBank.WebApi.Requests.Operations.Budget;
using PiggyBank.WebApi.Requests.Operations.Transfer;

namespace PiggyBank.WebApi.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class OperationsController : ControllerBase
    {
        private readonly IOperationService _service;

        public OperationsController(IOperationService service)
            => _service = service;

        [HttpGet]
        public Task<PageResult<OperationDto>> Get(int page, bool all = false, CancellationToken token = default)
        {
            if (page == default || page < 0)
                page = 1;

            var command = new GetOperationsCommand
            {
                Page = page,
                WithDeleted = all,
                UserId = User.GetUserId()
            };

            return _service.GetOperations(command, token);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOperations([FromQuery] int[] id, CancellationToken token)
        {
            var command = new DeleteOperationsCommand
            {
                Ids = id,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            await _service.DeleteOperations(command, token);
            return Ok();
        }

        #region Budget

        [HttpPost("Budget")]
        [InvalidStateFilter]
        public async Task<IActionResult> PostBudget(CreateBudgetOperationRequest request, CancellationToken token)
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

            await _service.AddBudgetOperation(command, token);

            return Ok();
        }

        [HttpDelete("Budget/{operationId}")]
        public async Task<IActionResult> DeleteBudgetOperation(int operationId, CancellationToken token)
        {
            var command = new DeleteBudgetOperationCommand
            {
                Id = operationId,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            await _service.DeleteBudgetOperation(command, token);
            return Ok();
        }

        [HttpPut("Budget/{operationId}")]
        [InvalidStateFilter]
        public async Task<IActionResult> UpdateBudget(int operationId, UpdateBudgetOperationRequest request, CancellationToken token)
        {
            var command = new UpdateBidgetOperationCommand
            {
                Id = operationId,
                AccountId = request.AccountId,
                CategoryId = request.CategoryId,
                Amount = request.Amount,
                Comment = request.Comment,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            await _service.UpdateBidgetOperation(command, token);

            return Ok();
        }

        [HttpPatch("Budget/{operationId}")]
        [InvalidStateFilter]
        public async Task<IActionResult> PartialUpdateBudget(int operationId, PartialUpdateBudgetOperationRequest request, CancellationToken token)
        {
            var command = new UpdatePartialBidgetOperationCommand
            {
                Id = operationId,
                AccountId = request.AccountId,
                CategoryId = request.CategoryId,
                Amount = request.Amount,
                Comment = request.Comment,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            await _service.UpdatePartialBidgetOperation(command, token);

            return Ok();
        }

        [HttpGet("Budget/{operationId}")]
        public Task<BudgetDto> GetBudgetOperation(int operationId, CancellationToken token)
        {
            var query = new GetOperationQuery
            {
                OperationId = operationId,
                UserId = User.GetUserId()
            };

            return _service.GetBudgetOperation(query, token);
        }

        #endregion

        #region Transfer

        [HttpPost("Transfer")]
        [InvalidStateFilter]
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

            await _service.AddTransferOperation(command, token);

            return Ok();
        }

        [HttpDelete("Transfer/{operationId}")]
        public async Task<IActionResult> DeleteTransferOperation(int operationId, CancellationToken token)
        {
            var command = new DeleteTransferOperationCommand
            {
                Id = operationId,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            await _service.DeleteTransferOperation(command, token);
            return Ok();
        }

        [HttpPut("Transfer/{operationId}")]
        [InvalidStateFilter]
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
                ModifiedOn = DateTime.UtcNow
            };

            await _service.UpdateTransferOperation(command, token);

            return Ok();
        }

        [HttpPatch("Transfer/{operationId}")]
        [InvalidStateFilter]
        public async Task<IActionResult> UpdatePartialTransferOperation(int operationId, PartialUpdateTransferOperationRequest request,
            CancellationToken token)
        {
            var command = new UpdatePartialTransferOperationCommand
            {
                Id = operationId,
                Amount = request.Amount,
                Comment = request.Comment,
                From = request.From,
                To = request.To,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            await _service.UpdatePartialTransferOperation(command, token);

            return Ok();
        }

        [HttpGet("Transfer/{operationId}")]
        public Task<TransferDto> GetTransferOperation(int operationId, CancellationToken token)
        {
            var query = new GetOperationQuery
            {
                OperationId = operationId,
                UserId = User.GetUserId()
            };

            return _service.GetTransferOperation(query, token);
        }

        #endregion
    }
}