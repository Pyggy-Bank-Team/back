﻿using System;
using System.Linq;
using System.Threading;
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
        private readonly bool _all;

        public GetAccountsQuery(PiggyContext context, Guid userId, bool all) : base(context)
            => (_userId, _all) = (userId, all);

        public override Task<AccountDto[]> Invoke(CancellationToken token)
        {
            var query = _all
                ? GetRepository<Account>().Where(a => a.CreatedBy == _userId)
                : GetRepository<Account>().Where(a => a.CreatedBy == _userId && !a.IsDeleted);

            return query.Select(a => new AccountDto
            {
                Id = a.Id,
                Type = a.Type,
                Balance = a.Balance,
                Currency = a.Currency,
                Title = a.Title,
                IsArchived = a.IsArchived,
                IsDeleted = a.IsDeleted,
                CreatedOn = a.CreatedOn,
                CreatedBy = a.CreatedBy
            }).ToArrayAsync(token);
        }
    }
}