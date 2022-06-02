using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PiggyBank.Common.Results;
using Serilog;

namespace PiggyBank.Domain.PipelineBehaviours
{
    public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>  where TRequest : IRequest<TResponse> 
                                                                                                        where TResponse : BaseResult, new()
    {
        private readonly ILogger _logger;

        public LoggingPipelineBehavior(ILogger logger)
            =>  _logger = logger;
        
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.Information("Handling {Handler}", nameof(IRequest));
            var response = await next();
            _logger.Information("Handled  {Handler}", nameof(IRequest));

            return response;
        }
    }
}