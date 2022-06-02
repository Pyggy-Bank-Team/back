using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.WebApi.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Accounts;
using Common.Queries;
using MediatR;
using PiggyBank.WebApi.Filters;
using PiggyBank.WebApi.Requests.Accounts;
using PiggyBank.WebApi.Responses;

namespace PiggyBank.WebApi.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
         private readonly IMediator _mediator;
        
         public AccountsController(IMediator mediator)
             => _mediator = mediator;
        
         [HttpGet]
         public async Task<IActionResult> Get(bool all = false, CancellationToken token = default)
         {
             var query = new GetAccountsQuery { All = all, UserId = User.GetUserId() };
             var result = await _mediator.Send(query, token);

             if (result.IsSuccess)
                 return Ok(result.Data);

             return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
         }

         [HttpPost, InvalidState]
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
        
             var result = await _mediator.Send(command, token);

             if (result.IsSuccess)
                 return Ok(result);

             return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
         }
        
         [InvalidState, HttpPut("{accountId}")]
         public async Task<IActionResult> Update(int accountId, UpdateAccountRequest request, CancellationToken token)
         {
             var command = new UpdateAccountCommand
             {
                 Id = accountId,
                 Balance = request.Balance,
                 Title = request.Title,
                 Type = request.Type,
                 IsArchived = request.IsArchived,
                 ModifiedBy = User.GetUserId(),
                 ModifiedOn = DateTime.UtcNow
             };
        
             var result = await _mediator.Send(command, token);

             if (result.IsSuccess)
                 return Ok();
             
             return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
         }
        
         [HttpPatch("{accountId}")]
         public async Task<IActionResult> PartialUpdate(int accountId, PartialUpdateAccountRequest request, CancellationToken token)
         {
             var command = new PartialUpdateAccountCommand
             {
                 Id = accountId,
                 Balance = request.Balance,
                 IsArchive = request.IsArchived,
                 Title = request.Title,
                 Type = request.Type,
                 ModifiedBy = User.GetUserId(),
                 ModifiedOn = DateTime.UtcNow
             };
        
             var result = await _mediator.Send(command, token);

             if (result.IsSuccess)
                 return Ok();

             return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
         }
        
         [HttpGet("{accountId}")]
         public async Task<IActionResult> GetById(int accountId, CancellationToken token)
         {
             var query = new GetAccountQuery { AccountId = accountId, UserId = User.GetUserId() };
             var result = await _mediator.Send(query, token);

             if (result.IsSuccess)
                 return Ok(result.Data);

             return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
         }

         [HttpDelete("{accountId}")]
         public async Task<IActionResult> Delete(int accountId, CancellationToken token)
         {
             var command = new DeleteAccountCommand
             {
                 Id = accountId,
                 ModifiedBy = User.GetUserId(),
                 ModifiedOn = DateTime.UtcNow
             };
        
             var result = await _mediator.Send(command, token);

             if (result.IsSuccess)
                 return Ok();

             return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
         }
        
         [HttpPatch("{accountId}/archive")]
         public async Task<IActionResult> Archive(int accountId, CancellationToken token)
         {
             var command = new ArchiveAccountCommand
             {
                 Id = accountId,
                 ModifiedBy = User.GetUserId(),
                 ModifiedOn = DateTime.UtcNow
             };
        
             var result = await _mediator.Send(command, token);

             if (result.IsSuccess)
                 return Ok();

             return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
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
        
             var result = await _mediator.Send(command, token);

             if (result.IsSuccess)
                 return Ok();

             return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
         }
    }
}