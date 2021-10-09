using PiggyBank.Domain.Infrastructure;
using PiggyBank.Model;
using Serilog;

namespace PiggyBank.Domain.Services
{
    public abstract class PiggyServiceBase
    {
        protected PiggyServiceBase(PiggyContext context, ILogger logger)
        {
            HandlerDispatcher = new HandlerDispatcher(context, logger);
            QueryDispatcher = new QueryDispatcher(context, logger);
        }

        protected HandlerDispatcher HandlerDispatcher { get; }

        protected QueryDispatcher QueryDispatcher { get; }
    }
}