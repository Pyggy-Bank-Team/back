using Microsoft.EntityFrameworkCore;
using PiggyBank.Domain.Infrastructure;
using PiggyBank.Model;

namespace PiggyBank.Domain.Services
{
    public partial class PiggyService
    {
        private readonly HandlerDispatcher _handlerDispatcher;
        private readonly QueryDispatcher _queryDispatcher;

        public PiggyService(PiggyContext context)
        {
            context.Database.Migrate();

            _handlerDispatcher = new HandlerDispatcher(context);
            _queryDispatcher = new QueryDispatcher(context);
        }
    }
}
