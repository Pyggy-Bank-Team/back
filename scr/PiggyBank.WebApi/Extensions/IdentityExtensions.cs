using System;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Principal;

namespace PiggyBank.WebApi.Extensions
{
    public static class IdentityExtensions
    {
        public static Guid GetUserId(this IPrincipal user)
        {
            var identity = GetClaimsIdentity(user);
            var claim = identity.FindFirst("id");
            return Guid.Parse(claim.Value);
        }

        public static string GetCurrency(this IPrincipal user)
        {
            var identity = GetClaimsIdentity(user);
            var claim = identity.FindFirst("currency");
            return claim.Value;
        }

        private static ClaimsIdentity GetClaimsIdentity(IPrincipal user)
        {
            switch (user.Identity)
            {
                case ClaimsIdentity identity when identity.IsAuthenticated:
                    return identity;
                default:
                    throw new AuthenticationException("No user is signed in.");
            }
        }
    }
}
