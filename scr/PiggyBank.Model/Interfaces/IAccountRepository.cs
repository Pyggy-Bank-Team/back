using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Model.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> AddAsync(Account newAccount, CancellationToken token);
    }
}