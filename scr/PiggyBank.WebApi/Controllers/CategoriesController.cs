using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.Common.Commands.Categories;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto;
using PiggyBank.WebApi.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.WebApi.Filters;
using PiggyBank.WebApi.Requests.Categories;

namespace PiggyBank.WebApi.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
            => _service = service;

        [HttpGet]
        public Task<CategoryDto[]> Get(bool all = false, CancellationToken token = default)
            => _service.GetCategories(User.GetUserId(), all, token);

        [HttpGet("{categoryId}")]
        public Task<CategoryDto> GetById(int categoryId, CancellationToken token)
            => _service.GetCategory(categoryId, token);

        [HttpPost, InvalidStateFilter]
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

            var newCategory = await _service.AddCategory(command, token);
            return Ok(newCategory);
        }

        [HttpPut("{categoryId}")]
        [InvalidStateFilter]
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

            await _service.UpdateCategory(command, token);

            return Ok();
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

            await _service.PartialUpdateCategory(command, token);

            return Ok();
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

            await _service.DeleteCategory(command, token);
            return Ok();
        }

        [HttpPatch("{categoryId}/Archive")]
        public async Task<IActionResult> Archive(int categoryId, CancellationToken token)
        {
            var userId = User.GetUserId();
            var command = new ArchiveCategoryCommand
            {
                Id = categoryId,
                ModifiedBy = userId,
                ModifiedOn = DateTime.UtcNow
            };

            await _service.ArchiveCategory(command, token);
            return Ok();
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

            await _service.DeleteCategories(command, token);
            return Ok();
        }
    }
}