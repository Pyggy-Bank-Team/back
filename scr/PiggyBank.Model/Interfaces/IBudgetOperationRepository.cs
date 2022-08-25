using System;
using System.Collections.Generic;
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
        IEnumerable<BudgetOperation> GetAll(Guid userId);
        IQueryable<BudgetOperation> GetAllAsQueryable(Guid userId);
        Task<BudgetOperation> UpdateAsync(BudgetOperation operation, CancellationToken token);
    }
}