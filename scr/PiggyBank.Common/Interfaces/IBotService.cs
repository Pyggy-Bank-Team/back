using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Telegram.Bot.Types;

namespace PiggyBank.Common.Interfaces
{
    public interface IBotService
    {
        Task ProcessUpdateCommand(Update command, ISession session, CancellationToken token);
    }
}