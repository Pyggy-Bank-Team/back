﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto;
using PiggyBank.WebApi.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PiggyBank.WebApi.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountsController(IAccountService service)
            => _service = service;

        [HttpGet]
        public Task<AccountDto[]> Get(CancellationToken token)
            => _service.GetAccounts(User.GetUserId(), token);

        [HttpPost]
        public async Task<IActionResult> Post(AccountDto request, CancellationToken token)
        {
            var command = new AddAccountCommand
            {
                Balance = request.Balance,
                Currency = request.Currency,
                Title = request.Title,
                Type = request.Type,
                CreatedBy = User.GetUserId(),
                CreatedOn = DateTime.UtcNow
            };

            await _service.AddAccount(command, token);

            return Ok();
        }

        [HttpPut, Route("{accountId}")]
        public async Task<IActionResult> Update(int accountId, AccountDto request, CancellationToken token)
        {
            var command = new UpdateAccountCommand
            {
                Balance = request.Balance,
                Currency = request.Currency,
                Id = accountId,
                Title = request.Title,
                Type = request.Type
            };

            await _service.UpdateAccount(command, token);

            return Ok();
        }

        [HttpPatch, Route("{accountId}")]
        public async Task<IActionResult> PartialUpdate(int accountId, PartialAccountDto request, CancellationToken token)
        {
            var command = new PartialUpdateAccountCommand
            {
                Id = request.Id ?? accountId,
                Balance = request.Balance,
                Currency = request.Currency,
                IsArchive = request.IsArchived,
                Title = request.Title,
                Type = request.Type
            };

            await _service.PartialUpdateAccount(command, token);

            return Ok();
        }

        [HttpGet, Route("{accountId}")]
        public Task<AccountDto> GetById(int accountId, CancellationToken token)
            => _service.GetAccount(accountId, token);

        [HttpDelete, Route("{accountId}")]
        public async Task<IActionResult> Delete(int accountId, CancellationToken token)
        {
            await _service.DeleteAccount(accountId, token);
            return Ok();
        }

        [HttpPatch, Route("{accountId}/Archive")]
        public async Task<IActionResult> Archive(int accountId, CancellationToken token)
        {
            await _service.ArchiveAccount(accountId, token);
            return Ok();
        }
    }
}
