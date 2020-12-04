using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Operations;
using PiggyBank.Common.Commands.Operations.Budget;
using PiggyBank.Common.Commands.Operations.Plan;
using PiggyBank.Common.Commands.Operations.Transfer;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models;
using PiggyBank.Common.Models.Dto.Operations;
using PiggyBank.Common.Queries;
using PiggyBank.Domain.Handler.Operations;
using PiggyBank.Domain.Handler.Operations.Budget;
using PiggyBank.Domain.Handler.Operations.Plan;
using PiggyBank.Domain.Handler.Operations.Transfer;
using PiggyBank.Domain.Queries.Operations;
using PiggyBank.Model;
using Serilog;

namespace PiggyBank.Domain.Services
{
    public class OperationService : ServiceBase, IOperationService
    {
        public OperationService(PiggyContext context, ILogger logger) : base(context, logger)
        {
        }
        
        public Task AddBudgetOperation(AddBudgetOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<AddBudgetOperationHandler, AddBudgetOperationCommand>(command, token);

        public Task AddPlanOperation(AddPlanOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<AddPlanOperationHandler, AddPlanOperationCommand>(command, token);

        public Task AddTransferOperation(AddTransferOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<AddTransferOperationHandler, AddTransferOperationCommand>(command, token);

        public Task ApplyPlanOperation(ApplyPlanOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<ApplyPlanOperationHandler, ApplyPlanOperationCommand>(command, token);

        public Task DeleteBudgetOperation(DeleteBudgetOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<DeleteBudgetOperationHandler, DeleteBudgetOperationCommand>(command, token);

        public Task DeletePlanOperation(DeletePlanOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<DeletePlanOperationHandler, DeletePlanOperationCommand>(command, token);

        public Task DeleteTransferOperation(DeleteTransferOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<DeleteTransferOperationHandler, DeleteTransferOperationCommand>(command, token);

        public Task<PageResult<OperationDto>> GetOperations(GetOperationsCommand command, CancellationToken token)
            => QueryDispatcher.Invoke<GetOperationsQuery, PageResult<OperationDto>>(token, command);

        public Task UpdateBidgetOperation(UpdateBidgetOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<UpdateBudgetOperationHandler, UpdateBidgetOperationCommand>(command, token);

        public Task UpdatePartialBidgetOperation(UpdatePartialBidgetOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<UpdatePartialBudgetOperationHandler, UpdatePartialBidgetOperationCommand>(command, token);

        public Task UpdateTransferOperation(UpdateTransferOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<UpdateTransferOperationHandler, UpdateTransferOperationCommand>(command, token);

        public Task UpdatePartialTransferOperation(UpdatePartialTransferOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<UpdatePartialTransferOperationHandler, UpdatePartialTransferOperationCommand>(command, token);

        public Task UpdatePlanOperation(UpdatePlanOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<UpdatePlanOperationHandler, UpdatePlanOperationCommand>(command, token);

        public Task UpdatePartialPlanOperation(UpdatePartialPlanOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<UpdatePartialPlanOperationHandler, UpdatePartialPlanOperationCommand>(command, token);

        public Task DeleteOperations(DeleteOperationsCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<DeleteOperationsHandler, DeleteOperationsCommand>(command, token);

        public Task<BudgetDto> GetBudgetOperation(GetOperationQuery query, CancellationToken token)
            => QueryDispatcher.Invoke<GetBudgetOperationQuery, BudgetDto>(token, query);

        public Task<TransferDto> GetTransferOperation(GetOperationQuery query, CancellationToken token)
            => QueryDispatcher.Invoke<GetTransferOperationQuery, TransferDto>(token, query);
    }
}