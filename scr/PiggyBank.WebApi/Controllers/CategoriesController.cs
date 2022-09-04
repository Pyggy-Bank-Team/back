using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.WebApi.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Categories;
using Common.Queries;
using MediatR;
using PiggyBank.WebApi.Filters;
using PiggyBank.WebApi.Requests.Categories;
using PiggyBank.WebApi.Responses;

namespace PiggyBank.WebApi.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken token)
        {
            var query = new GetCategoriesQuery { UserId = User.GetUserId() };
            var result = await _mediator.Send(query, token);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetById(int categoryId, CancellationToken token)
        {
            var query = new GetCategoryQuery { CategoryId = categoryId, UserId = User.GetUserId() };
            var result = await _mediator.Send(query, token);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
        }

        [HttpPost, ValidateRequest]
        public async Task<IActionResult> Post(CreateCategoryRequest request, CancellationToken token)
        {
            var command = new AddCategoryCommand
            {
                Title = request.Title,
                HexColor = request.HexColor,
                Type = request.Type,
                CreatedBy = User.GetUserId(),
                CreatedOn = DateTime.UtcNow,
                IsArchived = request.IsArchived
            };

            var result = await _mediator.Send(command, token);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
        }

        [ValidateRequest, HttpPut("{categoryId}")]
        public async Task<IActionResult> Update(int categoryId, UpdateCategoryRequest request, CancellationToken token)
        {
            var command = new UpdateCategoryCommand
            {
                Id = categoryId,
                Title = request.Title,
                HexColor = request.HexColor,
                IsArchived = request.IsArchived,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            var result = await _mediator.Send(command, token);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
        }

        [HttpPatch("{categoryId}")]
        public async Task<IActionResult> PartialUpdate(int categoryId, PartialUpdateCategoryRequest request, CancellationToken token)
        {
            var command = new PartialUpdateCategoryCommand
            {
                Id = categoryId,
                Title = request.Title,
                HexColor = request.HexColor,
                IsArchived = request.IsArchived,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            var result = await _mediator.Send(command, token);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId, CancellationToken token)
        {
            var command = new DeleteCategoryCommand
            {
                Id = categoryId,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            var result = await _mediator.Send(command, token);

            if (result.IsSuccess)
                return Ok();

            return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
        }

        [HttpPatch("{categoryId}/archive")]
        public async Task<IActionResult> Archive(int categoryId, CancellationToken token)
        {
            var command = new ArchiveCategoryCommand
            {
                Id = categoryId,
                ModifiedBy = User.GetUserId(),
                ModifiedOn = DateTime.UtcNow
            };

            var result = await _mediator.Send(command, token);

            if (result.IsSuccess)
                return Ok();

            return BadRequest(new ErrorResponse(result.ErrorCode, result.Messages));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategories([FromQuery] int[] id, CancellationToken token)
        {
            var command = new DeleteCategoriesCommand
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