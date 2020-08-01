﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Models.Dto;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Queries.Accounts
{
    public class GetAccountsQuery : BaseQuery<AccountDto[]>
    {
        private readonly Guid _userId;
        public GetAccountsQuery(PiggyContext context, Guid userId) : base(context)
            => _userId = userId;

        public override Task<AccountDto[]> Invoke()
            => GetRepository<Account>().Where(a => a.CreatedBy == _userId && !a.IsDeleted).Select(a => new AccountDto
            {
                Id = a.Id,
                Type = a.Type,
                Balance = a.Balance,
                Currency = a.Currency,
                Title = a.Title,
                IsArchived = a.IsArchived
            }).ToArrayAsync();
    }
}
