using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Model.Interfaces
{
    public interface IBudgetOperationRepository
    {
        Task<BudgetOperation> AddAsync(BudgetOperation operation, CancellationToken token);
        Task<BudgetOperation> GetAsync(int operationId, CancellationToken token);
        Task UpdateBudgetOperationAsync(BudgetOperation operation, CancellationToken token);
    }
}