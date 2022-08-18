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
    public class AccountRepository : IAccountRepository, IDisposable
    {
        private readonly PiggyContext _context;

        public AccountRepository(PiggyContext context)
            => _context = context;

        public async Task<Account> AddAsync(Account newAccount, CancellationToken token)
        {
            var efAccount = await _context.Accounts.AddAsync(newAccount, token);
            await _context.SaveChangesAsync(token);
            return efAccount.Entity;
        }

        public Task<Account> GetAsync(Guid userId, int accountId, CancellationToken token)
            => _context.Accounts.FirstOrDefaultAsync(a => a.CreatedBy == userId && a.Id == accountId, token);

        public async Task<Account> UpdateAsync(Account updatedAccount, CancellationToken token)
        {
            var efAccount = _context.Accounts.Update(updatedAccount);
            await _context.SaveChangesAsync(token);
            return efAccount.Entity;
        }

        public IEnumerable<Account> GetAllAsync(Guid userId)
            => _context.Accounts.Where(a => a.CreatedBy == userId);

        public void Dispose()
            => _context?.Dispose();
    }
}