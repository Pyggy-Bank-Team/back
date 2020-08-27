using PiggyBank.Common.Interfaces;
using PiggyBank.Domain.Queries.Operations;
using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Operations.Budget;
using PiggyBank.Common.Commands.Operations.Plan;
using PiggyBank.Common.Commands.Operations.Transfer;
using PiggyBank.Common.Models;
using PiggyBank.Common.Models.Dto.Operations;
using PiggyBank.Domain.Handler.Operations.Budget;
using PiggyBank.Domain.Handler.Operations.Plan;
using PiggyBank.Domain.Handler.Operations.Transfer;

namespace PiggyBank.Domain.Services
{
    public partial class PiggyService : IOperationService
    {
        public Task AddBudgetOperation(AddBudgetOperationCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<AddBudgetOperationHandler, AddBudgetOperationCommand>(command, token);

        public Task AddPlanOperation(AddPlanOperationCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<AddPlanOperationHandler, AddPlanOperationCommand>(command, token);

        public Task AddTransferOperation(AddTransferOperationCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<AddTransferOperationHandler, AddTransferOperationCommand>(command, token);

        public Task ApplyPlanOperation(int planOperationId, CancellationToken token)
            => _handlerDispatcher.Invoke<ApplyPlanOperationHandler, int>(planOperationId, token);

        public Task DeleteBudgetOperation(int id, CancellationToken token)
            => _handlerDispatcher.Invoke<DeleteBudgetOperationHandler, int>(id, token);

        public Task DeletePlanOperation(int operationId, CancellationToken token)
            => _handlerDispatcher.Invoke<DeletePlanOperationHandler, int>(operationId, token);

        public Task DeleteTransferOperation(int operationId, CancellationToken token)
            => _handlerDispatcher.Invoke<DeleteTransferOperationHandler, int>(operationId, token);

        public Task<OperationDto> GetOperation(int id, CancellationToken token)
            => _queryDispatcher.Invoke<GetOperationByIdQuery, OperationDto>(id);

        public Task<PageResult<OperationDto>> GetOperations(Guid userId, int page, CancellationToken token)
            => _queryDispatcher.Invoke<GetOperationsQuery, PageResult<OperationDto>>(userId, page);

        public Task UpdateBidgetOperation(UpdateBidgetOperationCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<UpdateBudgetOperationHandler, UpdateBidgetOperationCommand>(command, token);

        public Task UpdatePartialBidgetOperation(UpdatePartialBidgetOperationCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<UpdatePartialBudgetOperationHandler, UpdatePartialBidgetOperationCommand>(command, token);

        public Task UpdateTransferOperation(UpdateTransferOperationCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<UpdateTransferOperationHandler, UpdateTransferOperationCommand>(command, token);

        public Task UpdatePartialTransferOperation(UpdatePartialTransferOperationCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<UpdatePartialTransferOperationHandler, UpdatePartialTransferOperationCommand>(command, token);

        public Task UpdatePlanOperation(UpdatePlanOperationCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<UpdatePlanOperationHandler, UpdatePlanOperationCommand>(command, token);

        public Task UpdatePartialPlanOperation(UpdatePartialPlanOperationCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<UpdatePartialPlanOperationHandler, UpdatePartialPlanOperationCommand>(command, token);
    }
}
