using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Domain.Handler;
using PiggyBank.Model;
using Serilog;

namespace PiggyBank.Domain.Infrastructure
{
    public class HandlerDispatcher
    {
        private readonly PiggyContext _context;
        private readonly ILogger _logger;

        public HandlerDispatcher(PiggyContext context, ILogger logger)
            => (_context, _logger) = (context, logger);

        public async Task Invoke<THandler, TCommand>(TCommand command, CancellationToken token) where THandler : BaseHandler<TCommand>
        {
            using var handler = (BaseHandler<TCommand>)Activator.CreateInstance(typeof(THandler), _context, command);
            try
            {
                await handler.Invoke(token);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error during handler invoke");
                throw;
            }
            finally
            {
                await _context.SaveChangesAsync(token);
            }
        }
        
        public async Task<TResult> Invoke<THandler, TCommand, TResult>(TCommand command, CancellationToken token) where THandler : BaseHandler<TCommand>
                                                                                                                  where TResult : class, new()
        {
            using var handler = (BaseHandler<TCommand>)Activator.CreateInstance(typeof(THandler), _context, command);
            var result = new TResult();
            try
            {
                if (handler != null)
                {
                    await handler.Invoke(token);
                    
                    if (handler.Result is TResult handlerResult)
                        result = handlerResult;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error during handler invoke");
                throw;
            }
            finally
            {
                await _context.SaveChangesAsync(token);
            }
            
            return result;
        }
    }
}
