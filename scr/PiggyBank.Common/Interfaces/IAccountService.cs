using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Common.Models.Dto;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PiggyBank.Common.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Add a new account
        /// </summary>
        Task<AccountDto> AddAccount(AddAccountCommand command, CancellationToken token);

        /// <summary>
        /// Get accounts
        /// </summary>
        Task<AccountDto[]> GetAccounts(bool all, Guid userId, CancellationToken token);

        /// <summary>
        /// Get account by id
        /// </summary>
        Task<AccountDto> GetAccount(int id, CancellationToken token);

        /// <summary>
        /// Update exists entity
        /// </summary>
        Task UpdateAccount(UpdateAccountCommand command, CancellationToken token);

        /// <summary>
        /// Delete exists entity
        /// </summary>
        Task DeleteAccount(DeleteAccountCommand command, CancellationToken token);

        /// <summary>
        /// Archive exists entity
        /// </summary>
        Task ArchiveAccount(ArchiveAccountCommand command, CancellationToken token);

        Task PartialUpdateAccount(PartialUpdateAccountCommand command, CancellationToken token);
    }
}
