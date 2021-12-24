using PiggyBank.WebApi;
using Xunit;

namespace WebApi.Tests
{
    public class UsersControllerTest : IClassFixture<WebAppFactory<Startup>>
    {
        private readonly WebAppFactory<Startup> _factory;

        public UsersControllerTest(WebAppFactory<Startup> factory)
            => _factory = factory;

        [Fact]
        public void Post_Default_UserIsCreated()
        {
            var client = _factory.CreateClient();
        }
    }
}