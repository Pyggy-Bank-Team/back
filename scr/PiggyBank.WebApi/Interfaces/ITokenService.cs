using System.Threading.Tasks;
using PiggyBank.WebApi.Models;

namespace PiggyBank.WebApi.Interfaces
{
    public interface ITokenService
    {
        Task<Token> GetBearerToken(string userName, string password);
    }
}