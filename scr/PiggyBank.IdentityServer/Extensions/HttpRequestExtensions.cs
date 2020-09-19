using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace PiggyBank.IdentityServer.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetUserId(this HttpRequest request)
            => GetTokenFromRequest(request).Subject;

        public static JwtSecurityToken GetToken(this HttpRequest request)
            => GetTokenFromRequest(request);

        private static JwtSecurityToken GetTokenFromRequest(HttpRequest request)
        {
            var bearerToken = request.Headers["Authorization"];

            if (StringValues.IsNullOrEmpty(bearerToken) || bearerToken.Count > 1 || !bearerToken.Any(s => s.Contains("Bearer")))
                return null;

            var bearerTokenValue = bearerToken.First().Split(" ")[1];
            var  jwtToken = new JwtSecurityToken(bearerTokenValue);
            return jwtToken;
        }
    }
}