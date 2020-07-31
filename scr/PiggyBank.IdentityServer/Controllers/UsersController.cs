using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.IdentityServer.Dto;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.IdentityServer.Interfaces;

namespace PiggyBank.IdentityServer.Controllers
{
    [ApiController, Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public UsersController(UserManager<IdentityUser> userManager, ITokenService tokenService)
            => (_userManager, _tokenService) = (userManager, tokenService);

        [HttpPost]
        public async Task<IActionResult> Post(UserDto request, CancellationToken token)
        {
            var user = new IdentityUser {UserName = request.UserName};
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
    }
}