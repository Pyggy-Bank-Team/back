using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Operations;
using PiggyBank.Common.Commands.Operations.Budget;
using PiggyBank.Common.Commands.Operations.Transfer;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models;
using PiggyBank.Common.Models.Dto.Operations;
using PiggyBank.Common.Queries;
using PiggyBank.Domain.Handler.Operations;
using PiggyBank.Domain.Handler.Operations.Budget;
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
        
        public Task<BudgetDto> AddBudgetOperation(AddBudgetOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<AddBudgetOperationHandler, AddBudgetOperationCommand, BudgetDto>(command, token);

        public Task<TransferDto> AddTransferOperation(AddTransferOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<AddTransferOperationHandler, AddTransferOperationCommand, TransferDto>(command, token);

        public Task DeleteBudgetOperation(DeleteBudgetOperationCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<DeleteBudgetOperationHandler, DeleteBudgetOperationCommand>(command, token);

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

        public Task DeleteOperations(DeleteOperationsCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<DeleteOperationsHandler, DeleteOperationsCommand>(command, token);

        public Task<BudgetDto> GetBudgetOperation(GetOperationQuery query, CancellationToken token)
            => QueryDispatcher.Invoke<GetBudgetOperationQuery, BudgetDto>(token, query);

        public Task<TransferDto> GetTransferOperation(GetOperationQuery query, CancellationToken token)
            => QueryDispatcher.Invoke<GetTransferOperationQuery, TransferDto>(token, query);
    }
}