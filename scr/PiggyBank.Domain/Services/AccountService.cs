using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto;
using PiggyBank.Domain.Handler.Accounts;
using PiggyBank.Domain.Queries.Accounts;
using PiggyBank.Model;
using Serilog;

namespace PiggyBank.Domain.Services
{
    public class AccountService : ServiceBase, IAccountService
    {
        public AccountService(PiggyContext context, ILogger logger) : base(context, logger)
        {
        }
        
        public Task<AccountDto> AddAccount(AddAccountCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<AddAccountHandler, AddAccountCommand, AccountDto>(command, token);

        public Task ArchiveAccount(ArchiveAccountCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<ArchiveAccountHandler, ArchiveAccountCommand>(command, token);

        public Task DeleteAccount(DeleteAccountCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<DeleteAccountHandler, DeleteAccountCommand>(command, token);

        public Task<AccountDto> GetAccount(int accountId, CancellationToken token)
            => QueryDispatcher.Invoke<GetAccountByIdQuery, AccountDto>(token, accountId);

        public Task<AccountDto[]> GetAccounts(bool all, Guid userId, CancellationToken token)
            => QueryDispatcher.Invoke<GetAccountsQuery, AccountDto[]>(token, userId, all);

        public Task PartialUpdateAccount(PartialUpdateAccountCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<PartialUpdateAccountHandler, PartialUpdateAccountCommand>(command, token);

        public Task DeleteAccounts(DeleteAccountsCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<DeleteAccountsHandler, DeleteAccountsCommand>(command, token);

        public Task AddAccountBatch(AddAccountBatchCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<AddAccountBatchHandler, AddAccountBatchCommand>(command, token);

        public Task UpdateAccount(UpdateAccountCommand command, CancellationToken token)
            => HandlerDispatcher.Invoke<UpdateAccountHandler, UpdateAccountCommand>(command, token);
    }
}