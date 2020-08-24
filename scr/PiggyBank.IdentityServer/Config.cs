using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PiggyBank.IdentityServer
{
    public static class Config
    {
        public const string Issuer = "client";
        public const string Secret = "secret";
        public const int TokenLifetime = 3600 * 24 * 15;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
            => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {"api1", IdentityServerConstants.StandardScopes.OfflineAccess},
                    IdentityTokenLifetime = TokenLifetime,
                    AccessTokenLifetime = TokenLifetime * 2,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                }
            };
    }
}