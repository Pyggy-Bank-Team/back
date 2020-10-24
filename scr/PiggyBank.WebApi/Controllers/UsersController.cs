using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PiggyBank.IdentityServer.Models;
using PiggyBank.WebApi.Interfaces;
using PiggyBank.WebApi.Requests.Users;
using PiggyBank.WebApi.Responses;
using PiggyBank.WebApi.Responses.Tokens;
using TokenOptions = PiggyBank.WebApi.Options.TokenOptions;

namespace PiggyBank.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly TokenOptions _options;

        public UsersController(UserManager<ApplicationUser> userManager, ITokenService tokenService, IOptions<TokenOptions> options)
            => (_userManager, _tokenService, _options) = (userManager, tokenService, options.Value);
        
        [AllowAnonymous, HttpPost]
        public async Task<IActionResult> Post(CreateUserRequest request, CancellationToken token)
        {
            var user = new ApplicationUser {UserName = request.UserName, CurrencyBase = request.CurrencyBase};
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => new ErrorResponse(e.Code, e.Description)));

            var bearerToken = await _tokenService.GetBearerToken(request.UserName, request.Password);

            if (bearerToken)
            {
                return Ok(new TokenResponse
                {
                    AccessToken = bearerToken.Value,
                    ExpiresIn = _options.TokenLifetime,
                    TokenType = "BearerToken"
                });
            }

            return BadRequest(new ErrorResponse(bearerToken.ErrorType, "Can't create access token"));
        }
    }
}