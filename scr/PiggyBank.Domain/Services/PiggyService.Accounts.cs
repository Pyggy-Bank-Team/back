using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto;
using PiggyBank.Domain.Handler.Accounts;
using PiggyBank.Domain.Queries.Accounts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PiggyBank.Domain.Services
{
    public partial class PiggyService : IAccountService
    {
        public Task<AccountDto> AddAccount(AddAccountCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<AddAccountHandler, AddAccountCommand, AccountDto>(command, token);

        public Task ArchiveAccount(ArchiveAccountCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<ArchiveAccountHandler, ArchiveAccountCommand>(command, token);

        public Task DeleteAccount(DeleteAccountCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<DeleteAccountHandler, DeleteAccountCommand>(command, token);

        public Task<AccountDto> GetAccount(int accountId, CancellationToken token)
            => _queryDispatcher.Invoke<GetAccountByIdQuery, AccountDto>(token, accountId);

        public Task<AccountDto[]> GetAccounts(bool all, Guid userId, CancellationToken token)
            => _queryDispatcher.Invoke<GetAccountsQuery, AccountDto[]>(token, userId, all);

        public Task PartialUpdateAccount(PartialUpdateAccountCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<PartialUpdateAccountHandler, PartialUpdateAccountCommand>(command, token);

        public Task DeleteAccounts(DeleteAccountsCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<DeleteAccountsHandler, DeleteAccountsCommand>(command, token);

        public Task UpdateAccount(UpdateAccountCommand command, CancellationToken token)
            => _handlerDispatcher.Invoke<UpdateAccountHandler, UpdateAccountCommand>(command, token);
    }
}