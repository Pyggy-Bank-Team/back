using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.WebApi.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Operations;
using Common.Queries;
using Common.Results;
using Common.Results.Models;
using Common.Results.Models.Dto.Operations;
using MediatR;

namespace PiggyBank.WebApi.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class OperationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OperationsController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("page")]
        public Task<PageResult<OperationDto>> Get(int page, CancellationToken token)
        {
            if (page == default || page < 0)
                page = 1;

            var command = new GetOperationsQuery
            {
                Page = page,
                UserId = User.GetUserId()
            };

            return _service.GetOperations(command, token);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOperations([FromQuery] int[] id, CancellationToken token)
        {
            var command = new DeleteOperationsCommand
            {
                Ids = id,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            await _service.DeleteOperations(command, token);
            return Ok();
        }
    }
}