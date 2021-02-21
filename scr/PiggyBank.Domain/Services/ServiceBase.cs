using PiggyBank.Domain.Infrastructure;
using PiggyBank.Model;
using Serilog;

namespace PiggyBank.Domain.Services
{
    public abstract class ServiceBase
    {
        protected ServiceBase(PiggyContext context, ILogger logger)
        {
            HandlerDispatcher = new HandlerDispatcher(context, logger);
            QueryDispatcher = new QueryDispatcher(context, logger);
        }

        protected HandlerDispatcher HandlerDispatcher { get; }

        protected QueryDispatcher QueryDispatcher { get; }
    }
}