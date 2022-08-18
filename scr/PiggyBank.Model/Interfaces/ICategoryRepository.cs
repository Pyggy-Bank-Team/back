using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Model.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> AddAsync(Category newAccount, CancellationToken token);
        Task<Category> GetAsync(Guid userId, int accountId, CancellationToken token);
        Task<Category> UpdateAsync(Category updatedAccount, CancellationToken token);
        Task<IEnumerable<Category>> GetAllAsync(Guid userId, CancellationToken token);
    }
}