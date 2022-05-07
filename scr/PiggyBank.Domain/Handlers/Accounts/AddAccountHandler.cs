using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Models.Dto;
using PiggyBank.Domain.Commands.Accounts;
using PiggyBank.Domain.Results.Accounts;
using PiggyBank.Model.Interfaces;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Handlers.Accounts
{
    public class AddAccountHandler : IRequestHandler<AddAccountCommand, AddAccountResult>
    {
        private readonly IAccountRepository _repository;

        public AddAccountHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public override async Task Invoke(CancellationToken token)
        {
            var result = await GetRepository<Account>().AddAsync(new Account
            {
                Balance = DbLoggerCategory.Database.Command.Balance,
                Currency = DbLoggerCategory.Database.Command.Currency,
                IsArchived = DbLoggerCategory.Database.Command.IsArchived,
                Title = DbLoggerCategory.Database.Command.Title,
                Type = DbLoggerCategory.Database.Command.Type,
                CreatedBy = DbLoggerCategory.Database.Command.CreatedBy,
                CreatedOn = DbLoggerCategory.Database.Command.CreatedOn
            }, token);

            await SaveAsync();
            var account = result.Entity;
            Result = new AccountDto
            {
                Id = account.Id,
                Balance = account.Balance,
                Currency = account.Currency,
                Title = account.Title,
                Type = account.Type,
                IsArchived = account.IsArchived,
                CreatedBy = account.CreatedBy,
                CreatedOn = account.CreatedOn
            };
        }

        public async Task<AddAccountResult> Handle(AddAccountCommand request, CancellationToken cancellationToken)
        {
            var newAccount = await _repository.AddAsync(new Account(), cancellationToken);
        }
    }
}
