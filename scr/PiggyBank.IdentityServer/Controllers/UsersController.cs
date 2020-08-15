using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.IdentityServer.Dto;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.IdentityServer.Extensions;
using PiggyBank.IdentityServer.Interfaces;
using PiggyBank.IdentityServer.Models;

namespace PiggyBank.IdentityServer.Controllers
{
    [ApiController, Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public UsersController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
            => (_userManager, _tokenService) = (userManager, tokenService);

        [HttpPost]
        public async Task<IActionResult> Post(UserDto request, CancellationToken token)
        {
            var user = new ApplicationUser {UserName = request.UserName, CurrencyBase = request.CurrencyBase};
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var tokenResult = await _tokenService.GetBearerToken(user.UserName, request.Password, token);

            if (tokenResult == null)
            {
                var errorResponse = new
                {
                    code = "TokenIsNullOrEmpty",
                    description = "Can't generate token"
                };

                return BadRequest(errorResponse);
            }

            return Ok(tokenResult);
        }


        [HttpPatch, Route("currency")]
        public async Task<IActionResult> UpdateCurrency(CurrencyDto request, CancellationToken token)
        {
            var userId = Request.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                var errorResponse = new
                {
                    code = "NotFound",
                    description = "Bearer token not found"
                };
                return BadRequest(errorResponse);
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (request.PreviousCurrency != request.NewCurrency)
            {
                user.CurrencyBase = request.NewCurrency;
                await _userManager.UpdateAsync(user);
            }

            return Ok();
        }
    }
}