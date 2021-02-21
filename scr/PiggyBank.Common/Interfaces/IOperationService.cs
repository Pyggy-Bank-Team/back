using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Operations;
using PiggyBank.Common.Commands.Operations.Budget;
using PiggyBank.Common.Commands.Operations.Transfer;
using PiggyBank.Common.Models;
using PiggyBank.Common.Models.Dto.Operations;
using PiggyBank.Common.Queries;

namespace PiggyBank.Common.Interfaces
{
    public interface IOperationService
    {
        /// <summary>
        /// Add a new budget operation
        /// </summary>
        Task<BudgetDto> AddBudgetOperation(AddBudgetOperationCommand command, CancellationToken token);

        /// <summary>
        /// Add a new transfer operation
        /// </summary>
        Task<TransferDto> AddTransferOperation(AddTransferOperationCommand command, CancellationToken token);

        /// <summary>
        /// Get operations
        /// </summary>
        Task<PageResult<OperationDto>> GetOperations(GetOperationsCommand command, CancellationToken token);

        /// <summary>
        /// Delete budget operation
        /// </summary>
        Task DeleteBudgetOperation(DeleteBudgetOperationCommand command, CancellationToken token);

        /// <summary>
        /// Delete transfer operation
        /// </summary>
        Task DeleteTransferOperation(DeleteTransferOperationCommand command, CancellationToken token);

        Task UpdateBidgetOperation(UpdateBidgetOperationCommand command, CancellationToken token);

        Task UpdatePartialBidgetOperation(UpdatePartialBidgetOperationCommand command, CancellationToken token);

        Task UpdateTransferOperation(UpdateTransferOperationCommand command, CancellationToken token);

        Task UpdatePartialTransferOperation(UpdatePartialTransferOperationCommand command, CancellationToken token);

        Task DeleteOperations(DeleteOperationsCommand command, CancellationToken token);

        Task<BudgetDto> GetBudgetOperation(GetOperationQuery query, CancellationToken token);
        
        Task<TransferDto> GetTransferOperation(GetOperationQuery query, CancellationToken token);
    }
}
