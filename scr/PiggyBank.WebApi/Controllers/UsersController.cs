using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Common.Commands.Categories;
using PiggyBank.Common.Interfaces;
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
        private readonly IItemFactory _factory;
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;

        public UsersController(UserManager<ApplicationUser> userManager, ITokenService tokenService, IOptions<TokenOptions> options,
            IItemFactory factory, IAccountService accountService, ICategoryService categoryService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _options = options.Value;
            _factory = factory;
            _accountService = accountService;
            _categoryService = categoryService;
        }

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
                var accountBatchCommand = new AddAccountBatchCommand
                {
                    CreatedBy = Guid.Parse(user.Id),
                    CreatedOn = DateTime.UtcNow,
                    Accounts = _factory.GeAccountItems(request.Locale, request.CurrencyBase).ToArray()
                };

                await _accountService.AddAccountBatch(accountBatchCommand, token);

                var categoryBatchCommand = new AddCategoryBatchCommand
                {
                    CreatedBy = Guid.Parse(user.Id),
                    CreatedOn = DateTime.UtcNow,
                    Categories = _factory.GetCategoryItems(request.Locale).ToArray()
                };

                await _categoryService.AddCategoryBatch(categoryBatchCommand, token);

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

        [HttpGet("UserInfo")]
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