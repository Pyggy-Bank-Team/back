using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PiggyBank.IdentityServer.Interfaces;
using PiggyBank.IdentityServer.Responses;

namespace PiggyBank.IdentityServer.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _factory;

        public TokenService(IHttpClientFactory factory, IConfiguration configuration)
            => (_factory, _configuration) = (factory, configuration);

        public async Task<BearToken> GetBearerToken(string userName, string password, CancellationToken token)
        {
            var body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", "client"),
                new KeyValuePair<string, string>("client_secret", _configuration.GetSection("AppKeys")["ClientSecret"]),
                new KeyValuePair<string, string>("scope", _configuration.GetSection("AppKeys")["Scopes"]),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
            };

            using var client = _factory.CreateClient("GetToken");
            var baseUrl = _configuration.GetConnectionString("IdentityUrl");

            var response = await client.PostAsync($"{baseUrl}/connect/token", new FormUrlEncodedContent(body), token);

            if (!response.IsSuccessStatusCode)
                return null;

            return JsonConvert.DeserializeObject<BearToken>(await response.Content.ReadAsStringAsync());
        }
    }
}