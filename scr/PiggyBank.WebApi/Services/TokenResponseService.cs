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
using PiggyBank.WebApi.Configs;
using PiggyBank.WebApi.Interfaces;
using TokenResponse = PiggyBank.WebApi.Responses.Tokens.TokenResponse;

namespace PiggyBank.WebApi.Services
{
    public class TokenResponseService : ITokenResponseService
    {
        private readonly IOptions<TokenConfigs> _configs;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenResponseService(IOptions<TokenConfigs> configs, UserManager<ApplicationUser> userManager)
            => (_configs, _userManager) = (configs, userManager);
        
        public async Task<TokenResponse> GetBearerToken(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
                if (isValidPassword)
                {
                    var now = DateTime.UtcNow;
                    var jwt = new JwtSecurityToken(
                        issuer: _configs.Value.Issuer,
                        audience:_configs.Value.Audience,
                        notBefore: now,
                        claims:GetClaimsIdentity(user).Claims,
                        expires:now.AddDays(30),
                        signingCredentials:new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configs.Value.ClientSecret)), SecurityAlgorithms.HmacSha256)
                        );

                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                    return new TokenResponse
                    {
                        AccessToken = encodedJwt,
                        ExpiresIn = DateTimeOffset.Now.AddDays(30).ToUnixTimeSeconds(),
                        TokenType = "Bearer"
                    };
                }
            }

            return null;
        }

        private ClaimsIdentity GetClaimsIdentity(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultIssuer, _configs.Value.Issuer),
                new Claim("id", user.Id),
                new Claim("currency", user.CurrencyBase)
            };
            
            return new ClaimsIdentity(claims);
        }
    }
}