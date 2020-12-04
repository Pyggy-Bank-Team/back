using PiggyBank.Domain.Infrastructure;
using PiggyBank.Model;
using Serilog;

namespace PiggyBank.Domain.Services
{
    public abstract class ServiceBase
    {
        private readonly HandlerDispatcher _handlerDispatcher;
        private readonly QueryDispatcher _queryDispatcher;

        protected ServiceBase(PiggyContext context, ILogger logger)
        {
            _handlerDispatcher = new HandlerDispatcher(context, logger);
            _queryDispatcher = new QueryDispatcher(context, logger);
        }

        protected HandlerDispatcher HandlerDispatcher => _handlerDispatcher;
        protected QueryDispatcher QueryDispatcher => _queryDispatcher;
    }
}