using System.Threading;
using System.Threading.Tasks;
using PiggyBank.IdentityServer.Responses;

namespace PiggyBank.IdentityServer.Interfaces
{
    public interface ITokenService
    {
        Task<BearToken> GetBearerToken(string userName, string password, CancellationToken token);
    }
}