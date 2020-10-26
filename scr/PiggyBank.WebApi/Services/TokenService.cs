using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PiggyBank.IdentityServer.Models;
using PiggyBank.WebApi.Interfaces;
using PiggyBank.WebApi.Models;
using TokenOptions = PiggyBank.WebApi.Options.TokenOptions;

namespace PiggyBank.WebApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenOptions _options;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IOptions<TokenOptions> configs, UserManager<ApplicationUser> userManager)
            => (_options, _userManager) = (configs.Value, userManager);

        public async Task<Token> GetBearerToken(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return new Token {ErrorType = "UserNotFound"};

            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!isValidPassword)
                return new Token {ErrorType = "InvalidPassword"};

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                notBefore: now,
                claims: GetClaimsIdentity(user).Claims,
                expires: now.AddSeconds(_options.TokenLifetime),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.ClientSecret)),
                    SecurityAlgorithms.HmacSha256)
            );

            return new Token(new JwtSecurityTokenHandler().WriteToken(jwt));
        }

        private ClaimsIdentity GetClaimsIdentity(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultIssuer, _options.Issuer),
                new Claim("id", user.Id),
                new Claim("currency", user.CurrencyBase)
            };

            return new ClaimsIdentity(claims);
        }
    }
}