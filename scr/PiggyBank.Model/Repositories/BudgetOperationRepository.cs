using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Model.Interfaces;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Model.Repositories
{
    public class BudgetOperationRepository : IBudgetOperationRepository, IDisposable
    {
        private readonly PiggyContext _context;

        public BudgetOperationRepository(PiggyContext context)
            => _context = context;

        public async Task<BudgetOperation> AddAsync(BudgetOperation operation, CancellationToken token)
        {
            var efOperation = await _context.BudgetOperations.AddAsync(operation, token);
            await _context.SaveChangesAsync(token);
            return efOperation.Entity;
        }

        public Task<BudgetOperation> GetAsync(Guid userId, int operationId, CancellationToken token)
            => _context.BudgetOperations.FirstOrDefaultAsync(b => b.CreatedBy == userId && b.Id == operationId, token);

        public IEnumerable<BudgetOperation> GetAllAsync(Guid userId)
            => _context.BudgetOperations.Where(b => b.CreatedBy == userId);

        public async Task<BudgetOperation> UpdateAsync(BudgetOperation operation, CancellationToken token)
        {
            var efOperation = _context.BudgetOperations.Update(operation);
            await _context.SaveChangesAsync(token);
            return efOperation.Entity;
        }

        public void Dispose()
            => _context?.Dispose();
    }
}