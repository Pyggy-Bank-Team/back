using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Bot;

namespace PiggyBank.Common.Interfaces
{
    public interface IBotService
    {
        Task UpdateProcessing(UpdateCommand updateCommand, CancellationToken token);
    }
}