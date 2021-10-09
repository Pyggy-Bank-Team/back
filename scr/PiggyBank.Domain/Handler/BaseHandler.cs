using System.Threading.Tasks;
using PiggyBank.Domain.Infrastructure;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace PiggyBank.Domain.Handler
{
    public abstract class BaseHandler<TCommand> : DbWorker
    {
        public TCommand Command { get; set; }

        public object Result { get; set; }

        protected BaseHandler(DbContext context, TCommand command) : base(context)
            => Command = command;

        public abstract Task Invoke(CancellationToken token);
    }
}