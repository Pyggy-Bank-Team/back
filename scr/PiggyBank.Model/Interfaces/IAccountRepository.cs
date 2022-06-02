using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Model.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> AddAsync(Account newAccount, CancellationToken token);
        Task<Account> GetAsync(Guid userId, int accountId, CancellationToken token);
        Task<Account> UpdateAsync(Account updatedAccount, CancellationToken token);
        Task<IQueryable<Account>> GetAccountsAsync(Guid userId, CancellationToken token);
    }
}