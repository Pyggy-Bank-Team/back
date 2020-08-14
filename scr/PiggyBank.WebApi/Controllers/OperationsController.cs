﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.Common.Commands.Operations;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto;
using PiggyBank.WebApi.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Operations.Budget;
using PiggyBank.Common.Commands.Operations.Transfer;
using PiggyBank.Common.Models.Dto.Operations;

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
        public Task<OperationDto[]> Get(CancellationToken token)
            => _service.GetOperations(User.GetUserId(), token);

        #region Budget

        [HttpPost, Route("Budget")]
        public async Task<IActionResult> PostBudget(BudgetOperationDto request, CancellationToken token)
        {
            var command = new AddBudgetOperationCommand
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                CategoryId = request.CategoryId,
                Comment = request.Comment,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = User.GetUserId()
            };

            await _service.AddBudgetOperation(command, token);

            return Ok();
        }

        [HttpDelete, Route("Budget/{operationId}")]
        public async Task<IActionResult> DeleteBudgetOperation(int operationId, CancellationToken token)
        {
            await _service.DeleteBudgetOperation(operationId, token);
            return Ok();
        }

        [HttpPut, Route("{operationId}")]
        public async Task<IActionResult> UpdateBudget(int operationId, BudgetOperationDto request, CancellationToken token)
        {
            var command = new UpdateBidgetOperationCommand
            {
                Id = operationId,
                AccountId = request.AccountId,
                CategoryId = request.CategoryId,
                Amount = request.Amount,
                Comment = request.Comment
            };

            await _service.UpdateBidgetOperation(command, token);

            return Ok();
        }
        
        [HttpPatch, Route("{operationId}")]
        public async Task<IActionResult> PartialUpdateBudget(int operationId, PartialBudgetOperationDto request, CancellationToken token)
        {
            var command = new UpdatePartialBidgetOperationCommand
            {
                Id = operationId,
                AccountId = request.AccountId,
                CategoryId = request.CategoryId,
                Amount = request.Amount,
                Comment = request.Comment
            };

            await _service.UpdatePartialBidgetOperation(command, token);

            return Ok();
        }

        #endregion

        #region Transfer

        [HttpPost, Route("Transfer")]
        public async Task<IActionResult> PostTransfer(TransferOperationDto request, CancellationToken token)
        {
            var command = new AddTransferOperationCommand
            {
                Amount = request.Amount,
                From = request.From,
                To = request.To,
                Comment = request.Comment,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = User.GetUserId()
            };

            await _service.AddTransferOperation(command, token);

            return Ok();
        }

        [HttpDelete, Route("Transfer/{operationId}")]
        public async Task<IActionResult> DeleteTransferOperation(int operationId, CancellationToken token)
        {
            await _service.DeleteTransferOperation(operationId, token);
            return Ok();
        }

        [HttpPut, Route("Transfer/{operationId}")]
        public async Task<IActionResult> UpdateTransferOperation(int operationId, TransferOperationDto request, CancellationToken token)
        {
            var command = new UpdateTransferOperationCommand
            {
                Id = operationId,
                Amount = request.Amount,
                To = request.To,
                From = request.From,
                Comment = request.Comment
            };

            await _service.UpdateTransferOperation(command, token);

            return Ok();
        }

        #endregion

        #region Plan

        [HttpPost, Route("Plan")]
        public async Task<IActionResult> PostPlan(PlanOperationDto request, CancellationToken token)
        {
            var command = new AddPlanOperationCommand
            {
                Amount = request.Amount,
                CategoryId = request.CategoryId,
                Comment = request.Comment,
                PlanDate = request.PlanDate.GetValueOrDefault(),
                AccountId = request.AccountId,
                CreatedBy = User.GetUserId(),
                CreatedOn = DateTime.UtcNow
            };

            await _service.AddPlanOperation(command, token);

            return Ok();
        }

        [HttpPost, Route("Plan/{operationId}/Apply")]
        public async Task<IActionResult> ApplyPlanOperation(int operationId, CancellationToken token)
        {
            await _service.ApplyPlanOperation(operationId, token);
            return Ok();
        }

        [HttpDelete, Route("Plan/{operationId}/Delete")]
        public async Task<IActionResult> DeltePlanOperation(int operationId, CancellationToken token)
        {
            await _service.DeletePlanOperation(operationId, token);
            return Ok();
        }

        #endregion
    }
}
