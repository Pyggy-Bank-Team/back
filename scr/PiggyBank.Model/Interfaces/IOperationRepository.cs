using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Model.Interfaces
{
    public interface IOperationRepository
    {
        Task<IQueryable<BudgetOperation>> GetBudgetOperationsAsync(int accountId, CancellationToken token);
        Task<IQueryable<TransferOperation>> GetTransferOperationsAsync(int accountId, CancellationToken token);
        Task UpdateBudgetOperationAsync(BudgetOperation operation, CancellationToken token);
        Task UpdateTransferOperationAsync(TransferOperation operation, CancellationToken token);
    }
}