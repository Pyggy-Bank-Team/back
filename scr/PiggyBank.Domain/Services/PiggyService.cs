using PiggyBank.Domain.Infrastructure;
using PiggyBank.Model;
using Serilog;

namespace PiggyBank.Domain.Services
{
    public partial class PiggyService
    {
        private readonly HandlerDispatcher _handlerDispatcher;
        private readonly QueryDispatcher _queryDispatcher;

        public PiggyService(PiggyContext context, ILogger logger)
        {
            _handlerDispatcher = new HandlerDispatcher(context, logger);
            _queryDispatcher = new QueryDispatcher(context, logger);
        }
    }
}
