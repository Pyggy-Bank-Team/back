using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.IdentityServer.Dto;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
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
            var bearerToken = Request.Headers["Authorization"];

            if (StringValues.IsNullOrEmpty(bearerToken) || bearerToken.Count > 1 || !bearerToken.Any(s => s.Contains("Bearer")))
                return BadRequest();

            var bearerTokenValue = bearerToken.First().Split(" ")[1];
            var handler = new JwtSecurityToken(bearerTokenValue);

            var user = await _userManager.FindByIdAsync(handler.Subject);

            if (request.PreviousCurrency != request.NewCurrency)
            {
                user.CurrencyBase = request.NewCurrency;
                await _userManager.UpdateAsync(user);
            }

            return Ok();
        }
    }
}