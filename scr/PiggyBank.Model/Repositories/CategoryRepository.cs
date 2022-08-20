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
    public class CategoryRepository : ICategoryRepository, IDisposable
    {
        private readonly PiggyContext _context;

        public CategoryRepository(PiggyContext context)
            => _context = context;

        public async Task<Category> AddAsync(Category newAccount, CancellationToken token)
        {
            var efCategory = await _context.Categories.AddAsync(newAccount, token);
            await _context.SaveChangesAsync(token);
            return efCategory.Entity;
        }

        public Task<Category> GetAsync(Guid userId, int categoryId, CancellationToken token)
            => _context.Categories.FirstOrDefaultAsync(c => c.CreatedBy == userId && c.Id == categoryId, token);

        public async Task<Category> UpdateAsync(Category updatedCategory, CancellationToken token)
        {
            var efCategory = _context.Categories.Update(updatedCategory);
            await _context.SaveChangesAsync(token);
            return efCategory.Entity;
        }

        public IEnumerable<Category> GetAllAsync(Guid userId)
            => _context.Categories.Where(c => c.CreatedBy == userId);

        public void Dispose()
            => _context?.Dispose();
    }
}