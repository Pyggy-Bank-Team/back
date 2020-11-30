﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PiggyBank.WebApi.Extensions;
using PiggyBank.WebApi.Interfaces;
using PiggyBank.WebApi.Requests.Users;
using PiggyBank.WebApi.Responses;
using PiggyBank.WebApi.Responses.Tokens;
using PiggyBank.WebApi.Responses.Users;
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
            var user = new ApplicationUser {UserName = request.UserName, CurrencyBase = request.CurrencyBase, Email = request.Email};
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                var passwordInvalid = result.Errors.Any(e => e.Code.ToLowerInvariant().Contains("password"));
                return BadRequest(new ErrorResponse(passwordInvalid ? "PasswordInvalid" : "UserNotCreated", errors));
            }

            var bearerToken = await _tokenService.GetBearerToken(request.UserName, request.Password);

            if (bearerToken)
            {
                var tokenResponse = new TokenResponse
                {
                    AccessToken = bearerToken.Value,
                    ExpiresIn = _options.TokenLifetime,
                    TokenType = "BearerToken"
                };

                var userResponse = new UserResponse
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    CurrencyBase = user.CurrencyBase
                };

                return Ok(new {Token = tokenResponse, User = userResponse});
            }

            return BadRequest(new ErrorResponse(bearerToken.ErrorType, "Can't create access token"));
        }

        [HttpPatch]
        public async Task<IActionResult> Update(UpdateUserRequest request, CancellationToken token)
        {
            var userId = User.GetUserId();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return BadRequest(new ErrorResponse("UserNotFound", "User with this user id not found"));

            if (!string.IsNullOrEmpty(request.NewCurrency) && user.CurrencyBase != request.NewCurrency)
                user.CurrencyBase = request.NewCurrency;

            if (!string.IsNullOrEmpty(request.Email) && user.Email != request.Email)
                user.Email = request.Email;

            await _userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpGet, Route("UserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.GetUserId();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return BadRequest(new ErrorResponse("UserNotFound", "User with this user id not found"));

            return Ok(new UserResponse
            {
                UserName = user.UserName,
                Email = user.Email,
                CurrencyBase = user.CurrencyBase
            });
        }
    }
}