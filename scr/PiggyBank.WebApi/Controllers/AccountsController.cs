using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto;
using PiggyBank.WebApi.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.IdentityServer.Extensions;
using PiggyBank.IdentityServer.Models;

namespace PiggyBank.WebApi.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class AccountsController : ControllerBase, IDisposable
    {
        private readonly IAccountService _service;
        private readonly IdentityContext _identityContext;

        public AccountsController(IAccountService service, IdentityContext identityContext)
        {
            _service = service;
            _identityContext = identityContext;
        }

        [HttpGet]
        public Task<AccountDto[]> Get(bool all = false, CancellationToken token = default)
            => _service.GetAccounts(all, User.GetUserId(), token);

        [HttpPost]
        public async Task<IActionResult> Post(AccountDto request, CancellationToken token)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == User.GetUserId().ToString(), cancellationToken: token);

            if (user == null)
            {
                var errorResponse = new
                {
                    code = "UserNotFound",
                    description = "Can't found user"
                };

                return BadRequest(errorResponse);
            }

            var command = new AddAccountCommand
            {
                Balance = request.Balance,
                Currency = user.CurrencyBase,
                Title = request.Title,
                Type = request.Type,
                CreatedBy = User.GetUserId(),
                CreatedOn = DateTime.UtcNow,
                IsArchived = request.IsArchived
            };

            var result = await _service.AddAccount(command, token);

            return Ok(result);
        }

        [HttpPut, Route("{accountId}")]
        public async Task<IActionResult> Update(int accountId, AccountDto request, CancellationToken token)
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
        public async Task<IActionResult> PartialUpdate(int accountId, PartialAccountDto request, CancellationToken token)
        {
            var command = new PartialUpdateAccountCommand
            {
                Id = request.Id ?? accountId,
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
                UserId = userId,
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
                ModifiedOn = DateTime.UtcNow,
                UserId = userId
            };
            
            await _service.ArchiveAccount(command, token);
            return Ok();
        }

        public void Dispose()
        {
            _identityContext?.Dispose();
        }
    }
}