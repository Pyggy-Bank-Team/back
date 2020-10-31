using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto;
using PiggyBank.WebApi.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.WebApi.Filters;
using PiggyBank.WebApi.Requests.Accounts;

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
        public Task<AccountDto[]> Get(bool all = false, CancellationToken token = default)
            => _service.GetAccounts(all, User.GetUserId(), token);

        [HttpPost, InvalidStateFilter]
        public async Task<IActionResult> Post(CreateAccountRequest request, CancellationToken token)
        {
            var command = new AddAccountCommand
            {
                Balance = request.Balance,
                Currency = User.GetCurrency(),
                Title = request.Title,
                Type = request.Type,
                CreatedBy = User.GetUserId(),
                CreatedOn = DateTime.UtcNow,
                IsArchived = request.IsArchived
            };

            var result = await _service.AddAccount(command, token);

            return Ok(result);
        }

        [InvalidStateFilter]
        [HttpPut, Route("{accountId}")]
        public async Task<IActionResult> Update(int accountId, UpdateAccountRequest request, CancellationToken token)
        {
            var command = new UpdateAccountCommand
            {
                Id = accountId,
                Balance = request.Balance,
                Title = request.Title,
                Type = request.Type,
                IsArchived = request.IsArchived
            };

            await _service.UpdateAccount(command, token);

            return Ok();
        }

        [HttpPatch, Route("{accountId}")]
        public async Task<IActionResult> PartialUpdate(int accountId, PartialUpdateAccountRequest request, CancellationToken token)
        {
            var command = new PartialUpdateAccountCommand
            {
                Id = accountId,
                Balance = request.Balance,
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
            var userId = User.GetUserId();
            var command = new DeleteAccountCommand
            {
                Id = accountId,
                ModifiedBy = userId,
                ModifiedOn = DateTime.UtcNow
            };

            await _service.DeleteAccount(command, token);
            return Ok();
        }

        [HttpPatch, Route("{accountId}/Archive")]
        public async Task<IActionResult> Archive(int accountId, CancellationToken token)
        {
            var userId = User.GetUserId();
            var command = new ArchiveAccountCommand
            {
                Id = accountId,
                ModifiedBy = userId,
                ModifiedOn = DateTime.UtcNow
            };

            await _service.ArchiveAccount(command, token);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccounts([FromQuery] int[] id, CancellationToken token)
        {
            var command = new DeleteAccountsCommand
            {
                Ids = id,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            await _service.DeleteAccounts(command, token);
            return Ok();
        }
    }
}