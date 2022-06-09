using System;
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
        private readonly PiggyContext _piggyContext;

        public AccountRepository(PiggyContext piggyContext)
            => _piggyContext = piggyContext;

        public async Task<Account> AddAsync(Account newAccount, CancellationToken token)
        {
            var efAccount = await _piggyContext.Accounts.AddAsync(newAccount, token);
            await _piggyContext.SaveChangesAsync(token);
            return efAccount.Entity;
        }

        public Task<Account> GetAsync(Guid userId, int accountId, CancellationToken token)
            => _piggyContext.Accounts.FirstOrDefaultAsync(a => a.CreatedBy == userId && a.Id == accountId, token);

        public async Task<Account> UpdateAsync(Account updatedAccount, CancellationToken token)
        {
            var efAccount = _piggyContext.Accounts.Update(updatedAccount);
            await _piggyContext.SaveChangesAsync(token);
            return efAccount.Entity;
        }

        public IQueryable<Account> GetAccountsAsync(Guid userId)
            => _piggyContext.Accounts.Where(a => a.CreatedBy == userId);

        public void Dispose()
            => _piggyContext?.Dispose();
    }
}