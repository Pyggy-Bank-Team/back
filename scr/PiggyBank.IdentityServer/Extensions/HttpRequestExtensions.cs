using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace PiggyBank.IdentityServer.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetUserId(this HttpRequest request)
        {
            var bearerToken = request.Headers["Authorization"];

            if (StringValues.IsNullOrEmpty(bearerToken) || bearerToken.Count > 1 || !bearerToken.Any(s => s.Contains("Bearer")))
                return string.Empty;

            var bearerTokenValue = bearerToken.First().Split(" ")[1];
            var handler = new JwtSecurityToken(bearerTokenValue);
            return handler.Subject;
        }
    }
}