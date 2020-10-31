using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PiggyBank.WebApi.Filters;
using PiggyBank.WebApi.Interfaces;
using PiggyBank.WebApi.Options;
using PiggyBank.WebApi.Requests.Tokens;
using PiggyBank.WebApi.Responses;
using PiggyBank.WebApi.Responses.Tokens;

namespace PiggyBank.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class TokensController : ControllerBase
    {
        private readonly ITokenService _token;
        private readonly TokenOptions _options;

        public TokensController(ITokenService service, IOptions<TokenOptions> options)
            => (_token, _options) = (service, options.Value);

        [AllowAnonymous, InvalidStateFilter]
        [HttpPost, Route("Connect")]
        public async Task<IActionResult> Connect(GetTokenRequest request, CancellationToken token)
        {
            var bearerToken = await _token.GetBearerToken(request.UserName, request.Password);

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