using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Domain.Infrastructure;
using PiggyBank.Model;

namespace PiggyBank.Domain.QueriesHandlers
{
    public abstract class BaseQuery<TOutput> : DbWorker
    {
        protected BaseQuery(PiggyContext context) : base(context) {}

        public abstract Task<TOutput> Invoke(CancellationToken token);
    }
}
