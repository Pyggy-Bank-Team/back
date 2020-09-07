using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Domain.Handler;
using PiggyBank.Model;

namespace PiggyBank.Domain.Infrastructure
{
    public class HandlerDispatcher
    {
        private readonly PiggyContext _context;

        public HandlerDispatcher(PiggyContext context)
            => _context = context;

        public async Task Invoke<THandler, TCommand>(TCommand command, CancellationToken token) where THandler : BaseHandler<TCommand>
        {
            using var handler = (BaseHandler<TCommand>)Activator.CreateInstance(typeof(THandler), _context, command);
            try
            {
                await handler.Invoke(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                //TODO Added log
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
