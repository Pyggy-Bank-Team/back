using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Model.Interfaces
{
    public interface IBudgetOperationRepository
    {
        Task<BudgetOperation> AddAsync(BudgetOperation operation, CancellationToken token);
        Task<BudgetOperation> GetAsync(Guid userId, int operationId, CancellationToken token);
        Task<IQueryable<BudgetOperation>> GetAllAsync(Guid userId, CancellationToken token);
        Task<BudgetOperation> UpdateAsync(BudgetOperation operation, CancellationToken token);
    }
}