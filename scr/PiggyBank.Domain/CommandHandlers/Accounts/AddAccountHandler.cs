﻿using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Accounts;
using Common.Results.Accounts;
using Common.Results.Models.Dto;
using MediatR;
using PiggyBank.Model.Interfaces;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.CommandHandlers.Accounts
{
    public class AddAccountHandler : IRequestHandler<AddAccountCommand, AddAccountResult>
    {
        private readonly IAccountRepository _repository;

        public AddAccountHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<AddAccountResult> Handle(AddAccountCommand request, CancellationToken cancellationToken)
        {
            var newAccount = new Account
            {
                Balance = request.Balance,
                Currency = request.Currency,
                IsArchived = request.IsArchived,
                Title = request.Title,
                Type = request.Type,
                CreatedBy = request.CreatedBy,
                CreatedOn = request.CreatedOn
            };

            var createdAccount = await _repository.AddAsync(newAccount, cancellationToken);

            return new AddAccountResult
            {
                Data = new AccountDto
                {
                    Id = createdAccount.Id,
                    Balance = createdAccount.Balance,
                    Currency = createdAccount.Currency,
                    Title = createdAccount.Title,
                    Type = createdAccount.Type,
                    IsArchived = createdAccount.IsArchived,
                    CreatedBy = createdAccount.CreatedBy,
                    CreatedOn = createdAccount.CreatedOn,
                    IsDeleted = createdAccount.IsDeleted
                }
            };
        }
    }
}