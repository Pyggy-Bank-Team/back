using System.Net;
using PiggyBank.WebApi;
using PiggyBank.WebApi.Requests.Users;
using WebApi.Tests.Extensions;
using Xunit;

namespace WebApi.Tests
{
    public class UsersControllerTest : IClassFixture<PiggyAppFactory<Startup>>
    {
        private readonly PiggyAppFactory<Startup> _factory;

        public UsersControllerTest(PiggyAppFactory<Startup> factory)
            => _factory = factory;

        [Fact]
        public async void Post_Default_UserIsCreated()
        {
            var createUserRequest = new CreateUserRequest
            {
                Email = "test@piggybank.pro",
                Locale = "en-NZ",
                Password = "2Y*iTmpa",
                CurrencyBase = "NZD",
                UserName = "tester"
            };
            using var client = _factory.CreateClient();

            var httpResponse = await client.PostAsync("/api/users", createUserRequest.ToStringContent());
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        }
    }
}