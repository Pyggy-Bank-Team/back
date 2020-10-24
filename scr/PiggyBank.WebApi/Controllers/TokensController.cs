using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.WebApi.Interfaces;
using PiggyBank.WebApi.Requests.Tokens;

namespace PiggyBank.WebApi.Controllers
{
    [AllowAnonymous]
    [ApiController, Route("api/[controller]")]
    public class TokensController : ControllerBase
    {
        private readonly ITokenResponseService _tokenResponse;

        public TokensController(ITokenResponseService responseService)
            => _tokenResponse = responseService;
        
        [HttpPost, Route("Connect")]
        public async Task<IActionResult> Connect(GetTokenRequest request, CancellationToken token)
        {
            var result = await _tokenResponse.GetBearerToken(request.UserName, request.Password);
            return Ok(result);
        }
    }
}