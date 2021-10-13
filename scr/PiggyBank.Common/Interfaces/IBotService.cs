using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace PiggyBank.Common.Interfaces
{
    public interface IBotService
    {
        Task<string> ProcessUpdateCommand(Update command, string operationSnapshotJson, CancellationToken token);
    }
}