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
        Task<Category> GetAsync(Guid userId, int categoryId, CancellationToken token);
        Task<Category> UpdateAsync(Category updatedCategory, CancellationToken token);
        IEnumerable<Category> GetAllAsync(Guid userId);
    }
}