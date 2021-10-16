using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Domain.Queries;
using Serilog;

namespace PiggyBank.Domain.Infrastructure
{
    public class QueryDispatcher
    {
        private readonly DbContext _context;
        private readonly ILogger _logger;

        public QueryDispatcher(DbContext context, ILogger logger)
            => (_context, _logger) = (context, logger);

        public Task<TOutput> Invoke<TQuery, TOutput>(CancellationToken token, object param1, object param2) where TQuery : BaseQuery<TOutput>
            => PrivateInvoke<TQuery, TOutput>(token, _context, param1, param2);

        public Task<TOutput> Invoke<TQuery, TOutput>(CancellationToken token, object param1) where TQuery : BaseQuery<TOutput>
            => PrivateInvoke<TQuery, TOutput>(token, _context, param1);

        public Task<TOutput> Invoke<TQuery, TOutput>(CancellationToken token) where TQuery : BaseQuery<TOutput>
            => PrivateInvoke<TQuery, TOutput>(token, _context);

        private async Task<TOutput> PrivateInvoke<TQuery, TOutput>(CancellationToken token, params object[] obj)
            where TQuery : BaseQuery<TOutput>
        {
            TOutput result;
            using var query = (TQuery) Activator.CreateInstance(typeof(TQuery), obj);
            try
            {
                result = await query.Invoke(token);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error during query invoke");
                throw;
            }

            return result;
        }
    }
}