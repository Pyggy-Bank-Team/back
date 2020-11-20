﻿using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Operations;
using PiggyBank.Common.Commands.Operations.Budget;
using PiggyBank.Common.Commands.Operations.Plan;
using PiggyBank.Common.Commands.Operations.Transfer;
using PiggyBank.Common.Models;
using PiggyBank.Common.Models.Dto;

namespace PiggyBank.Common.Interfaces
{
    public interface IOperationService
    {
        /// <summary>
        /// Add a new budget operation
        /// </summary>
        Task AddBudgetOperation(AddBudgetOperationCommand command, CancellationToken token);

        /// <summary>
        /// Add a new transfer operation
        /// </summary>
        Task AddTransferOperation(AddTransferOperationCommand command, CancellationToken token);

        /// <summary>
        /// Add a new plan operation
        /// </summary>
        Task AddPlanOperation(AddPlanOperationCommand command, CancellationToken token);

        /// <summary>
        /// Apply exists plan operation
        /// </summary>
        Task ApplyPlanOperation(ApplyPlanOperationCommand command, CancellationToken token);

        /// <summary>
        /// Get operations
        /// </summary>
        Task<PageResult<OperationDto>> GetOperations(GetOperationsCommand command, CancellationToken token);

        /// <summary>
        /// Get operation by id
        /// </summary>
        Task<OperationDto> GetOperation(int id, CancellationToken token);

        /// <summary>
        /// Delete budget operation
        /// </summary>
        Task DeleteBudgetOperation(DeleteBudgetOperationCommand command, CancellationToken token);

        /// <summary>
        /// Delete plan operation
        /// </summary>
        Task DeletePlanOperation(DeletePlanOperationCommand command, CancellationToken token);

        /// <summary>
        /// Delete transfer operation
        /// </summary>
        Task DeleteTransferOperation(DeleteTransferOperationCommand command, CancellationToken token);

        Task UpdateBidgetOperation(UpdateBidgetOperationCommand command, CancellationToken token);

        Task UpdatePartialBidgetOperation(UpdatePartialBidgetOperationCommand command, CancellationToken token);

        Task UpdateTransferOperation(UpdateTransferOperationCommand command, CancellationToken token);

        Task UpdatePartialTransferOperation(UpdatePartialTransferOperationCommand command, CancellationToken token);

        Task UpdatePlanOperation(UpdatePlanOperationCommand command, CancellationToken token);

        Task UpdatePartialPlanOperation(UpdatePartialPlanOperationCommand command, CancellationToken token);

        Task DeleteOperations(DeleteOperationsCommand command, CancellationToken token);
    }
}
