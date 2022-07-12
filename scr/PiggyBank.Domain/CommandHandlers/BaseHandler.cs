using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Domain.Infrastructure;

namespace PiggyBank.Domain.CommandHandlers
{
    public abstract class BaseHandler : DbWorker
    {
        protected BaseHandler(DbContext context) : base(context) { }
        
        public abstract Task Invoke(CancellationToken token);
    }
    
    public abstract class BaseHandler<TCommand> : BaseHandler
    {
        public TCommand Command { get; set; }

        public object Result { get; set; }

        protected BaseHandler(DbContext context, TCommand command) : base(context)
            => Command = command;
    }
}