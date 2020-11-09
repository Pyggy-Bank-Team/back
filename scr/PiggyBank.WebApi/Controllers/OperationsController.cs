using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.Common.Interfaces;
using PiggyBank.WebApi.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Operations;
using PiggyBank.Common.Commands.Operations.Budget;
using PiggyBank.Common.Commands.Operations.Plan;
using PiggyBank.Common.Commands.Operations.Transfer;
using PiggyBank.Common.Models;
using PiggyBank.Common.Models.Dto;
using PiggyBank.WebApi.Filters;
using PiggyBank.WebApi.Requests.Operations.Budget;
using PiggyBank.WebApi.Requests.Operations.Plan;
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
        public Task<PageResult<OperationDto>> Get(int page, CancellationToken token)
        {
            if (page == default || page < 0)
                page = 1;
            
            return _service.GetOperations(User.GetUserId(), page, token);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOperations([FromQuery]int[] id, CancellationToken token)
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

        [HttpPost, Route("Budget")]
        [InvalidStateFilter]
        public async Task<IActionResult> PostBudget(CreateBudgetOperationRequest request, CancellationToken token)
        {
            var command = new AddBudgetOperationCommand
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                CategoryId = request.CategoryId,
                Comment = request.Comment,
                CreatedOn = request.CreatedOn ?? DateTime.UtcNow,
                CreatedBy = User.GetUserId()
            };

            await _service.AddBudgetOperation(command, token);

            return Ok();
        }

        [HttpDelete, Route("Budget/{operationId}")]
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

        [HttpPut, Route("Budget/{operationId}")]
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
        
        [HttpPatch, Route("Budget/{operationId}")]
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

        #endregion

        #region Transfer

        [HttpPost, Route("Transfer")]
        [InvalidStateFilter]
        public async Task<IActionResult> PostTransfer(CreateTransferOperationRequest request, CancellationToken token)
        {
            var command = new AddTransferOperationCommand
            {
                Amount = request.Amount,
                From = request.From,
                To = request.To,
                Comment = request.Comment,
                CreatedOn = request.CreatedOn ?? DateTime.UtcNow,
                CreatedBy = User.GetUserId()
            };

            await _service.AddTransferOperation(command, token);

            return Ok();
        }

        [HttpDelete, Route("Transfer/{operationId}")]
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

        [HttpPut, Route("Transfer/{operationId}")]
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

        [HttpPatch, Route("Transfer/{operationId}")]
        [InvalidStateFilter]
        public async Task<IActionResult> UpdatePartialTransferOperation(int operationId, PartialUpdateTransferOperationRequest request, CancellationToken token)
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

        #endregion

        #region Plan

        [HttpPost, Route("Plan")]
        [InvalidStateFilter]
        public async Task<IActionResult> PostPlan(CreatePlanOperationRequest request, CancellationToken token)
        {
            var command = new AddPlanOperationCommand
            {
                Amount = request.Amount,
                CategoryId = request.CategoryId,
                Comment = request.Comment,
                PlanDate = request.PlanDate,
                AccountId = request.AccountId,
                CreatedBy = User.GetUserId(),
                CreatedOn = request.CreatedOn ?? DateTime.UtcNow
            };

            await _service.AddPlanOperation(command, token);

            return Ok();
        }

        [HttpPost, Route("Plan/{operationId}/Apply")]
        public async Task<IActionResult> ApplyPlanOperation(int operationId, CancellationToken token)
        {
            var command = new ApplyPlanOperationCommand
            {
                Id = operationId,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };
            
            await _service.ApplyPlanOperation(command, token);
            return Ok();
        }

        [HttpDelete, Route("Plan/{operationId}")]
        public async Task<IActionResult> DeletePlanOperation(int operationId, CancellationToken token)
        {
            var command = new DeletePlanOperationCommand
            {
                Id = operationId,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };
            
            await _service.DeletePlanOperation(command, token);
            return Ok();
        }

        [HttpPut, Route("Plan/{operationId}")]
        [InvalidStateFilter]
        public async Task<IActionResult> UpdatePlanOperation(int operationId, UpdatePlanOperationRequest request, CancellationToken token)
        {
            var command = new UpdatePlanOperationCommand
            {
                Id = operationId,
                Amount = request.Amount,
                Comment = request.Comment,
                AccountId = request.AccountId,
                CategoryId = request.CategoryId,
                PlanDate = request.PlanDate,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            await _service.UpdatePlanOperation(command, token);
            return Ok();
        }
        
        [HttpPatch, Route("Plan/{operationId}")]
        [InvalidStateFilter]
        public async Task<IActionResult> UpdatePartialPlanOperation(int operationId, PartialUpdatePlanOperationRequest request, CancellationToken token)
        {
            var command = new UpdatePartialPlanOperationCommand
            {
                Id = operationId,
                Amount = request.Amount,
                Comment = request.Comment,
                AccountId = request.AccountId,
                CategoryId = request.CategoryId,
                PlanDate = request.PlanDate,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            await _service.UpdatePartialPlanOperation(command, token);
            return Ok();
        }

        #endregion
    }
}
