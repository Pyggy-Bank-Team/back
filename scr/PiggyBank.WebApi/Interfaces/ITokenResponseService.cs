using System.Threading;
using System.Threading.Tasks;
using PiggyBank.WebApi.Responses.Tokens;

namespace PiggyBank.WebApi.Interfaces
{
    public interface ITokenResponseService
    {
        Task<TokenResponse> GetBearerToken(string userName, string password);
    }
}